using MGisbert.Appointments.Data.Enums;
using MGisbert.Appointments.Models;
using MGisbert.Appointments.Models.Request;
using Moq;
using System.Linq.Expressions;

namespace MGisbert.Appointments.Tests
{
    public class AppointmentServiceTests : IClassFixture<AppointmentServiceFixture>
    {
        private readonly AppointmentServiceFixture _fixture;

        public AppointmentServiceTests(AppointmentServiceFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AddAppointmentAsync_UserExists_ReturnsAppointment()
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest { UserId = 1, Title = "Test", Date = DateTime.Now.AddDays(1) };
            var appointmentEntity = new Data.Entities.Appointment();
            var appointmentModel = new Appointment();

            _fixture.UserRepositoryMock.Setup(repo => repo.GetAny(It.IsAny<Expression<Func<Data.Entities.User, bool>>>()))
                .ReturnsAsync(true);
            _fixture.MapperMock.Setup(mapper => mapper.Map<Data.Entities.Appointment>(appointmentRequest))
                .Returns(appointmentEntity);
            _fixture.AppointmentRepositoryMock.Setup(repo => repo.AddAsync(appointmentEntity, false))
                .Returns(Task.CompletedTask);
            _fixture.MapperMock.Setup(mapper => mapper.Map<Appointment>(appointmentEntity))
                .Returns(appointmentModel);

            // Act
            var result = await _fixture.AppointmentService.AddAppointmentAsync(appointmentRequest);

            // Assert
            Assert.Equal(appointmentModel, result);
        }

        [Fact]
        public async Task AddAppointmentAsync_UserDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var appointmentRequest = new AppointmentRequest { UserId = 1, Title = "Test" };

            _fixture.UserRepositoryMock.Setup(repo => repo.GetAny(It.IsAny<Expression<Func<Data.Entities.User, bool>>>()))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _fixture.AppointmentService.AddAppointmentAsync(appointmentRequest));
        }

        [Fact]
        public async Task DeleteAppointmentAsync_AppointmentExistsAndPending_DeletesAppointment()
        {
            // Arrange
            var appointmentEntity = new Data.Entities.Appointment { Id = 1, UserId = 1, Status = Status.Pending };

            _fixture.AppointmentRepositoryMock.Setup(repo => repo.GetSingleASync(It.IsAny<Expression<Func<Data.Entities.Appointment, bool>>>()))
                .ReturnsAsync(appointmentEntity);
            _fixture.AppointmentRepositoryMock.Setup(repo => repo.DeleteAsync(appointmentEntity, true))
                .Returns(Task.CompletedTask);

            // Act
            await _fixture.AppointmentService.DeleteAppointmentAsync(1, 1);

            // Assert
            _fixture.AppointmentRepositoryMock.Verify(repo => repo.DeleteAsync(appointmentEntity, true), Times.Once);
        }

        [Fact]
        public async Task DeleteAppointmentAsync_AppointmentNotPending_ThrowsInvalidOperationException()
        {
            // Arrange
            var appointmentEntity = new Data.Entities.Appointment { Id = 1, UserId = 1, Status = Status.Approved };

            _fixture.AppointmentRepositoryMock.Setup(repo => repo.GetSingleASync(It.IsAny<Expression<Func<Data.Entities.Appointment, bool>>>()))
                .ReturnsAsync(appointmentEntity);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _fixture.AppointmentService.DeleteAppointmentAsync(1, 1));
        }

        [Fact]
        public async Task GetAppointmentAsync_AppointmentExists_ReturnsAppointment()
        {
            // Arrange
            var appointmentEntity = new Data.Entities.Appointment { Id = 1, UserId = 1 };
            var appointmentModel = new Appointment();

            _fixture.AppointmentRepositoryMock.Setup(repo => repo.GetSingleASync(It.IsAny<Expression<Func<Data.Entities.Appointment, bool>>>()))
                .ReturnsAsync(appointmentEntity);
            _fixture.MapperMock.Setup(mapper => mapper.Map<Appointment>(appointmentEntity))
                .Returns(appointmentModel);

            // Act
            var result = await _fixture.AppointmentService.GetAppointmentAsync(1, 1);

            // Assert
            Assert.Equal(appointmentModel, result);
        }

        [Fact]
        public async Task GetAppointmentsByUserIdAsync_ValidSortParameter_ReturnsSortedAppointments()
        {
            // Arrange
            var appointmentsFromDatabase = new List<Data.Entities.Appointment>
                {
                    new Data.Entities.Appointment { Title = "title1", Date = DateTime.Now, Status = Status.Pending },
                    new Data.Entities.Appointment { Title = "title2", Date = DateTime.Now.AddDays(1), Status = Status.Approved }
                };

            var appointmentsFromResponse = new List<Appointment>
                {
                    new Appointment { Title = "title1", Date = DateTime.Now, Status = Status.Pending },
                    new Appointment { Title = "title2", Date = DateTime.Now.AddDays(1), Status = Status.Approved }
                };

            _fixture.AppointmentRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Expression<Func<Data.Entities.Appointment, bool>>>()))
                .ReturnsAsync(appointmentsFromDatabase);
            _fixture.MapperMock.Setup(mapper => mapper.Map<IEnumerable<Appointment>>(appointmentsFromDatabase))
                .Returns(appointmentsFromResponse);

            // Act
            var result = await _fixture.AppointmentService.GetAppointmentsByUserIdAsync(1, "date", true);
            var comparer = appointmentsFromResponse.OrderBy(a => a.Date).ToList();
            // Assert
            Assert.Equal(comparer, result.ToList());
        }

        [Fact]
        public async Task UpdateAppointmentAsync_AppointmentExistsAndPending_UpdatesAppointment()
        {
            // Arrange
            var appointmentEntity = new Data.Entities.Appointment { Id = 1, UserId = 1, Status = Status.Pending };
            var appointmentModel = new Appointment { Id = 1, UserId = 1 };

            _fixture.AppointmentRepositoryMock.Setup(repo => repo.GetSingleASync(It.IsAny<Expression<Func<Data.Entities.Appointment, bool>>>()))
                .ReturnsAsync(appointmentEntity);
            _fixture.MapperMock.Setup(mapper => mapper.Map(appointmentModel, appointmentEntity))
                .Returns(appointmentEntity);
            _fixture.AppointmentRepositoryMock.Setup(repo => repo.UpdateAsync(appointmentEntity, false))
                .Returns(Task.CompletedTask);
            _fixture.MapperMock.Setup(mapper => mapper.Map<Appointment>(appointmentEntity))
                .Returns(appointmentModel);

            // Act
            var result = await _fixture.AppointmentService.UpdateAppointmentAsync(appointmentModel, 1);

            // Assert
            Assert.Equal(appointmentModel, result);
        }

        [Fact]
        public async Task UpdateAppointmentStatusAsync_AppointmentExists_UpdatesStatus()
        {
            // Arrange
            var appointmentEntity = new Data.Entities.Appointment { Id = 1, UserId = 1, Status = Status.Pending };
            var appointmentModel = new Appointment { Id = 1, UserId = 1, Status = Status.Approved };

            var updatedStatus = Status.Approved;

            _fixture.AppointmentRepositoryMock.Setup(repo => repo.GetAsync(1))
                .ReturnsAsync(appointmentEntity);
            _fixture.AppointmentRepositoryMock.Setup(repo => repo.UpdateAsync(appointmentEntity, true))
                .Returns(Task.CompletedTask);
            _fixture.MapperMock.Setup(mapper => mapper.Map<Appointment>(appointmentEntity))
                .Returns(appointmentModel);

            // Act
            var result = await _fixture.AppointmentService.UpdateAppointmentStatusAsync(1, updatedStatus);

            // Assert
            Assert.Equal(updatedStatus, result.Status);
            _fixture.AppointmentRepositoryMock.Verify(repo => repo.UpdateAsync(appointmentEntity, true), Times.Once);
        }
    }
}
using AutoMapper;
using MGisbert.Appointments.Data.Repositories;
using MGisbert.Appointments.Services.Implementation;
using Microsoft.Extensions.Logging;
using Moq;

namespace MGisbert.Appointments.Tests
{
    public class AppointmentServiceFixture : IDisposable
    {
        public Mock<IAppointmentRepository> AppointmentRepositoryMock { get; private set; }
        public Mock<IUserRepository> UserRepositoryMock { get; private set; }
        public Mock<IMapper> MapperMock { get; private set; }
        public Mock<ILogger<AppointmentService>> LoggerMock { get; private set; }
        public AppointmentService AppointmentService { get; private set; }

        public AppointmentServiceFixture()
        {
            AppointmentRepositoryMock = new Mock<IAppointmentRepository>();
            UserRepositoryMock = new Mock<IUserRepository>();
            MapperMock = new Mock<IMapper>();
            LoggerMock = new Mock<ILogger<AppointmentService>>();
            AppointmentService = new AppointmentService(
                AppointmentRepositoryMock.Object,
                UserRepositoryMock.Object,
                MapperMock.Object,
                LoggerMock.Object
            );
        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}

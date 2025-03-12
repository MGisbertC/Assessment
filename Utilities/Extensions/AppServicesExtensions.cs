using MGisbert.Appointments.Data.Repositories;
using MGisbert.Appointments.Data.Repositories.Implementation;
using MGisbert.Appointments.Services;
using MGisbert.Appointments.Services.Implementation;

namespace MGisbert.Appointments.Utilities.Extensions
{
    public static class AppServicesExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            //Services
            services.AddScoped<IAppointmentService, AppointmentService>();

            //Repositories
            services.AddScoped<IAppointmentRepository, AppointmentsRepository>();
            services.AddScoped<IUserRepository, UsersRepository>();
            services.AddScoped<IRoleRepository, RolesRepository>();
        }
    }
}

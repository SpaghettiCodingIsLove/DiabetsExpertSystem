using DiabetsAPI.Models.Requests;
using DiabetsAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace DiabetsAPI.DB
{
    public static class DbInitializer
    {
        public static void Initialize(DiabetsContext context, IUserService userSerivce)
        {
            if (context.Database.EnsureCreated())
            {
                userSerivce.CreateDoctor(new CreateDoctorRequest()
                {
                    Name = "admin",
                    LastName = "admin",
                    Login = "admin",
                    Password = "admin",
                    IsAdmin = true
                });
            }
            else
            {
                context.Database.Migrate();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace DiabetsAPI.DB
{
    public static class DbInitializer
    {
        public static void Initialize(DiabetsContext context)
        {
            if (context.Database.EnsureCreated())
            {
                context.Doctors.Add(new Doctor()
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

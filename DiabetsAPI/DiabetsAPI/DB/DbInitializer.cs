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
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    IsAdmin = true
                });

                context.SaveChanges();
            }
            else
            {
                context.Database.Migrate();
            }
        }
    }
}

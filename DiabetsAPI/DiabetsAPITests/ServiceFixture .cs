using AutoMapper;
using DiabetsAPI.DB;
using DiabetsAPI.Helpers;
using DiabetsAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BC = BCrypt.Net.BCrypt;

namespace DiabetsAPITests
{
    public class ServiceFixture : IDisposable
    {
        private DbContextOptions<DiabetsContext> dbContextOptions = new DbContextOptionsBuilder<DiabetsContext>()
            .UseInMemoryDatabase(databaseName: "PrimeDb")
            .Options;

        public DiabetsContext Context { get; private set; }
        public IUserService Controller { get; private set; }
        public ICryptoService CryptoService { get; private set; }

        public ServiceFixture()
        {
            CryptoService = new CryptoService(Options.Create(new AppSettings() { Secret = "XOjy4Z2w/JLcYznUutvvoQ==" }));

            Context = SeedDb();

            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            Controller = new UserService(Context, config.CreateMapper(), CryptoService);
        }

        private DiabetsContext SeedDb()
        {
            DiabetsContext context = new DiabetsContext(dbContextOptions);
            List<Doctor> doctors = new List<Doctor>
            {
                new Doctor { Id = 1, Name = "TestD1", LastName = "TestD1", Login = "TestD1", Password = BC.HashPassword("TestD1"), IsAdmin = true },
                new Doctor { Id = 2, Name = "TestD2", LastName = "TestD2", Login = "TestD2", Password = BC.HashPassword("TestD2"), IsAdmin = false }
            };

            List<Patient> patients = new List<Patient>
            {
                new Patient { Id = 1, Name = CryptoService.Encrypt("TestP1"), LastName = CryptoService.Encrypt("TestP2"), Pesel = CryptoService.Encrypt("123456"), BirthDate = new DateOnly(2000, 1, 1) },
            };

            List<Examination> examinations = new List<Examination>
            {
                new Examination { Id = 1, Date = new DateTime(2022, 10, 10), DoctorId = 1, PatientId = 1, Measures =  CryptoService.Encrypt($"20;20;20;20;20;20;20;20;1") }
            };

            context.AddRange(doctors);
            context.AddRange(patients);
            context.AddRange(examinations);

            context.SaveChanges();

            return context;
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}

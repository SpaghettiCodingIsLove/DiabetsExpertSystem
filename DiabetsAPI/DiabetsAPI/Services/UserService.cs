using AutoMapper;
using DiabetsAPI.DB;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

namespace DiabetsAPI.Services
{
    public class UserService : IUserService
    {
        private readonly DiabetsContext context;
        private readonly IMapper mapper;
        private readonly ICryptoService cryptoService;

        public UserService(DiabetsContext context, IMapper mapper, ICryptoService cryptoService)
        {
            this.context = context;
            this.mapper = mapper;
            this.cryptoService = cryptoService;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest)
        {
            if (string.IsNullOrWhiteSpace(authenticateRequest.Login) || string.IsNullOrWhiteSpace(authenticateRequest.Password))
            {
                return null;
            }

            Doctor user = context.Doctors.FirstOrDefault(x => authenticateRequest.Login.Equals(x.Login));

            if (user == null || !BC.Verify(authenticateRequest.Password, user.Password))
            {
                return null;
            }

            AuthenticateResponse response = mapper.Map<AuthenticateResponse>(user);

            return response;
        }

        public Doctor CreateDoctor(CreateDoctorRequest createDoctorRequest)
        {
            if (string.IsNullOrWhiteSpace(createDoctorRequest.Password))
            {
                return null;
            }

            if (!context.Doctors.Where(x => x.Login.Equals(createDoctorRequest.Login)).ToList().IsNullOrEmpty())
            {
                return null;
            }

            Doctor doctor = mapper.Map<Doctor>(createDoctorRequest);

            doctor.Password = BC.HashPassword(createDoctorRequest.Password);

            context.Doctors.Add(doctor);
            context.SaveChanges();

            return doctor;
        }

        public Patient CreatePatient(CreatePatientRequest createPatientRequest)
        {
            if (string.IsNullOrWhiteSpace(createPatientRequest.Pesel))
            {
                return null;
            }

            if (context.Patients.ToList().Any(x => cryptoService.Decrypt(x.Pesel).Equals(createPatientRequest.Pesel)))
            {
                return null;
            }

            Patient patient = mapper.Map<Patient>(createPatientRequest);

            patient.Pesel = cryptoService.Encrypt(createPatientRequest.Pesel);
            patient.Name = cryptoService.Encrypt(createPatientRequest.Name);
            patient.LastName = cryptoService.Encrypt(createPatientRequest.LastName);

            context.Patients.Add(patient);
            context.SaveChanges();

            return patient;
        }

        public void DeleteDoctor(int id)
        {
            Doctor user = context.Doctors.Find(id);

            if (user != null)
            {
                context.Doctors.Remove(user);
                context.SaveChanges();
            }
        }

        public void DeletePatient(int id)
        {
            Patient user = context.Patients.Find(id);

            if (user != null)
            {
                context.Patients.Remove(user);
                context.SaveChanges();
            }
        }
    }
}

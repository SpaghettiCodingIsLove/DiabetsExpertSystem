using AutoMapper;
using DiabetsAPI.DB;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public IEnumerable<PatientResponse> GetPatients()
        {
            List<PatientResponse> patients = new List<PatientResponse>();

            foreach (Patient patient in context.Patients)
            {
                PatientResponse patientResponse = new PatientResponse()
                {
                    Id = patient.Id,
                    Name = cryptoService.Decrypt(patient.Name),
                    LastName = cryptoService.Decrypt(patient.LastName),
                    BirthDate = patient.BirthDate.ToDateTime(new System.TimeOnly(0)),
                    Pesel = cryptoService.Decrypt(patient.Pesel)
                };

                patients.Add(patientResponse);
            }

            return patients;
        }

        public void AddExamination(AddExaminationRequest addExaminationRequest)
        {
            Examination examination = new Examination()
            {
                DoctorId = addExaminationRequest.DoctorId,
                PatientId = addExaminationRequest.PatientId,
                Date = DateTime.UtcNow,
                Measures = cryptoService.Encrypt($"{addExaminationRequest.Pregnancies};{addExaminationRequest.Glucose};{addExaminationRequest.BloodPreasure};{addExaminationRequest.SkinThickness};{addExaminationRequest.Insulin};{addExaminationRequest.Weight.ToString(CultureInfo.InvariantCulture)};{addExaminationRequest.Height.ToString(CultureInfo.InvariantCulture)};{addExaminationRequest.DiabetesPedigreeFunction.ToString(CultureInfo.InvariantCulture)};{addExaminationRequest.Outcome};")
            };

            context.Examinations.Add(examination);
            context.SaveChanges();
        }

        public IEnumerable<ExaminationResponse> GetExaminations(long patientId)
        {
            List<ExaminationResponse> examinations = new List<ExaminationResponse>();

            foreach (Examination examination in context.Examinations.Where(x => x.PatientId == patientId))
            {
                string[] measures = cryptoService.Decrypt(examination.Measures).Split(';');

                ExaminationResponse examinationResponse = new ExaminationResponse()
                {
                    Date = examination.Date,
                    Pregnancies = int.Parse(measures[0]),
                    Glucose = int.Parse(measures[1]),
                    BloodPreasure = int.Parse(measures[2]),
                    SkinThickness = int.Parse(measures[3]),
                    Insulin = int.Parse(measures[4]),
                    Weight = double.Parse(measures[5], CultureInfo.InvariantCulture),
                    Height = double.Parse(measures[6], CultureInfo.InvariantCulture),
                    DiabetesPedigreeFunction = double.Parse(measures[7], CultureInfo.InvariantCulture),
                    Outcome = int.Parse(measures[8])
                };

                examinations.Add(examinationResponse);
            }

            return examinations;
        }

        public bool ChangePassword(ChangePasswordRequest changePasswordRequest, long id)
        {
            if (string.IsNullOrWhiteSpace(changePasswordRequest.OldPassword) || string.IsNullOrWhiteSpace(changePasswordRequest.NewPassword))
            {
                return false;
            }

            Doctor user = context.Doctors.Find(id);

            if (user == null || !BC.Verify(changePasswordRequest.OldPassword, user.Password))
            {
                return false;
            }

            user.Password = BC.HashPassword(changePasswordRequest.NewPassword);
            context.SaveChanges();

            return true;
        }
    }
}

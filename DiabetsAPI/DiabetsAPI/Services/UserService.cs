using AutoMapper;
using DiabetsAPI.DB;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using BC = BCrypt.Net.BCrypt;
using File = System.IO.File;

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

        public PatientResponse CreatePatient(CreatePatientRequest createPatientRequest)
        {
            if (string.IsNullOrWhiteSpace(createPatientRequest.Pesel))
            {
                return null;
            }

            if (context.Patients.ToList().Any(x => cryptoService.Decrypt(x.Pesel).Equals(createPatientRequest.Pesel)))
            {
                return null;
            }

            Patient patient = new Patient()
            {
                BirthDate = DateOnly.FromDateTime(createPatientRequest.BirthDate)
            };

            patient.Pesel = cryptoService.Encrypt(createPatientRequest.Pesel);
            patient.Name = cryptoService.Encrypt(createPatientRequest.Name);
            patient.LastName = cryptoService.Encrypt(createPatientRequest.LastName);

            context.Patients.Add(patient);
            context.SaveChanges();

            PatientResponse patientResponse = mapper.Map<PatientResponse>(createPatientRequest);
            patientResponse.Id = patient.Id;

            return patientResponse;
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
                    BirthDate = patient.BirthDate.ToDateTime(new TimeOnly(0)),
                    Pesel = cryptoService.Decrypt(patient.Pesel)
                };

                patients.Add(patientResponse);
            }

            return patients;
        }

        public ExaminationResponse AddExamination(AddExaminationRequest addExaminationRequest)
        {
            Patient patient = context.Patients.Find(addExaminationRequest.PatientId);
            int age = (int)((DateTime.UtcNow - patient.BirthDate.ToDateTime(new TimeOnly(0))).TotalDays / 365.255);

            double bmi = addExaminationRequest.Weight / Math.Pow(addExaminationRequest.Height / 100, 2);
            string patientData = $"{addExaminationRequest.Pregnancies},{addExaminationRequest.Glucose},{addExaminationRequest.BloodPreasure},{addExaminationRequest.SkinThickness},{addExaminationRequest.Insulin},{bmi.ToString(CultureInfo.InvariantCulture)},{addExaminationRequest.DiabetesPedigreeFunction.ToString(CultureInfo.InvariantCulture)},{age}";

            DirectoryInfo directory = new DirectoryInfo("model");
            FileInfo myFile = directory.GetFiles()
                .OrderByDescending(x => x.LastWriteTime)
                .First();

            string cmdCommand;
#if DEBUG
            cmdCommand = $"python classify.py \"{myFile.FullName}\" \"{patientData}\"";
#else
            cmdCommand = $"classify.exe \"{myFile.FullName}\" \"{patientData}\"";
#endif
            int outcom = (int)double.Parse(RunCmd(cmdCommand).Trim().Replace(',', '.'), CultureInfo.InvariantCulture);

            Examination examination = new Examination()
            {
                DoctorId = addExaminationRequest.DoctorId,
                PatientId = addExaminationRequest.PatientId,
                Date = DateTime.UtcNow,
                Measures = cryptoService.Encrypt($"{addExaminationRequest.Pregnancies};{addExaminationRequest.Glucose};{addExaminationRequest.BloodPreasure};{addExaminationRequest.SkinThickness};{addExaminationRequest.Insulin};{addExaminationRequest.Weight.ToString(CultureInfo.InvariantCulture)};{addExaminationRequest.Height.ToString(CultureInfo.InvariantCulture)};{addExaminationRequest.DiabetesPedigreeFunction.ToString(CultureInfo.InvariantCulture)};{outcom}")
            };

            context.Examinations.Add(examination);
            context.SaveChanges();

            ExaminationResponse examinationResponse = mapper.Map<ExaminationResponse>(addExaminationRequest);
            examinationResponse.Outcome = outcom;
            examinationResponse.Date = examination.Date;

            return examinationResponse;
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

        public TrainingResponse Train(TrainingRequest trainingRequest)
        {
            if (!Directory.Exists("tmp"))
            {
                Directory.CreateDirectory("tmp");
            }

            if (!Directory.Exists("model"))
            {
                Directory.CreateDirectory("model");
            }

            string currTime = DateTime.UtcNow.ToString("dd_MM_yyyy_HH_mm_ss");
            string tmpFile = @$"tmp\{currTime}.csv";
            File.WriteAllText(tmpFile, trainingRequest.DataSet);

            string cmdCommand;
#if DEBUG
            cmdCommand = $"python train.py  \"{tmpFile}\" \"{@$"model\{currTime}.sav"}\"";
#else
            cmdCommand = $"train.exe  \"{tmpFile}\" \"{@$"model\{currTime}.sav"}\"";
#endif

            double score = double.Parse(RunCmd(cmdCommand));

            File.Delete(tmpFile);

            TrainingResponse trainingResponse = new TrainingResponse()
            {
                Score = score
            };

            return trainingResponse;
        }

        private string RunCmd(string command)
        {
            using (Process process = new Process())
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo()
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = $"/C {command}",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                };

                process.StartInfo = processStartInfo;

                process.Start();

                process.WaitForExit(5000);

                return process.StandardOutput.ReadToEnd(); ;
            }
        }
    }
}

using DiabetsAPI.DB;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DiabetsAPI.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);
        Doctor CreateDoctor(CreateDoctorRequest createDoctorRequest);
        PatientResponse CreatePatient(CreatePatientRequest createPatientRequest);
        void DeleteDoctor(long id);
        void DeletePatient(long id);
        IEnumerable<PatientResponse> GetPatients();
        ExaminationResponse AddExamination(AddExaminationRequest addExaminationRequest);
        IEnumerable<ExaminationResponse> GetExaminations(long patientId);
        bool ChangePassword(ChangePasswordRequest changePasswordRequest, long id);
        TrainingResponse Train(TrainingRequest trainingRequest);
    }
}

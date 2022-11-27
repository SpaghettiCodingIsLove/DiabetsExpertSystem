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
        void DeleteDoctor(int id);
        void DeletePatient(int id);
        IEnumerable<PatientResponse> GetPatients();
        void AddExamination(AddExaminationRequest addExaminationRequest);
        IEnumerable<ExaminationResponse> GetExaminations(long patientId);
        bool ChangePassword(ChangePasswordRequest changePasswordRequest, long id);
    }
}

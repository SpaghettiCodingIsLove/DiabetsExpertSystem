using DiabetsAPI.DB;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;
using System.Collections.Generic;

namespace DiabetsAPI.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest authenticateRequest);
        Doctor CreateDoctor(CreateDoctorRequest createDoctorRequest);
        Patient CreatePatient(CreatePatientRequest createPatientRequest);
        void DeleteDoctor(int id);
        void DeletePatient(int id);
        IEnumerable<PatientResponse> GetPatients();
        void AddExamination(AddExaminationRequest addExaminationRequest);
        IEnumerable<ExaminationResponse> GetExaminations(long patientId);
    }
}

using AutoMapper;
using DiabetsAPI.DB;
using DiabetsAPI.Models.Requests;
using DiabetsAPI.Models.Responses;

namespace DiabetsAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateDoctorRequest, Doctor>();
            CreateMap<CreatePatientRequest, Patient>();
            CreateMap<Doctor, AuthenticateResponse>();
        }
    }
}

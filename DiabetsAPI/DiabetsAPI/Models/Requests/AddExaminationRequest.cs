namespace DiabetsAPI.Models.Requests
{
    public class AddExaminationRequest
    {
        public long DoctorId { get; set; }
        public long PatientId { get; set; }
        public int Pregnancies { get; set; }
        public int Glucose { get; set; }
        public int BloodPreasure { get; set; }
        public int SkinThickness { get; set; }
        public int Insulin { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double DiabetesPedigreeFunction { get; set; }
    }
}

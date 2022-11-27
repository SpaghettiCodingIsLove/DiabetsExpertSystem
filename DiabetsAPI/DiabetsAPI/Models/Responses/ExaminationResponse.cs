using System;

namespace DiabetsAPI.Models.Responses
{
    public class ExaminationResponse
    {
        public DateTime Date { get; set; }
        public int Pregnancies { get; set; }
        public int Glucose { get; set; }
        public int BloodPreasure { get; set; }
        public int SkinThickness { get; set; }
        public int Insulin { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double DiabetesPedigreeFunction { get; set; }
        public int Outcome { get; set; }
    }
}

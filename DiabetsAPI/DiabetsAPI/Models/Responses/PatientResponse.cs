using System;

namespace DiabetsAPI.Models.Responses
{
    public class PatientResponse
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Pesel { get; set; }

        public DateTime BirthDate { get; set; }
    }
}

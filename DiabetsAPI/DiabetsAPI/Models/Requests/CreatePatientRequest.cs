using System;

namespace DiabetsAPI.Models.Requests
{
    public class CreatePatientRequest
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Pesel { get; set; }

        public DateOnly BirthDate { get; set; }
    }
}

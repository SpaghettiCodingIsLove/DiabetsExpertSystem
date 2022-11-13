namespace DiabetsAPI.Models.Requests
{
    public class CreateDoctorRequest
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
}

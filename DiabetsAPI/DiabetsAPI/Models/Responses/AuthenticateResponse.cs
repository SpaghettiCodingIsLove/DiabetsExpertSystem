namespace DiabetsAPI.Models.Responses
{
    public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }
}

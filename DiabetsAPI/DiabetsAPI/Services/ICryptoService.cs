namespace DiabetsAPI.Services
{
    public interface ICryptoService
    {
        string Encrypt(string data);
        string Decrypt(string data);
    }
}

using System.Security.Cryptography;

var randomNumber = new byte[16];

using (var rng = RandomNumberGenerator.Create())
{
    rng.GetBytes(randomNumber);
    Console.WriteLine(Convert.ToBase64String(randomNumber));
}
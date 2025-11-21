using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    // Simplemente una clase plana (POCO)
    public class LoginRequest
    {
       public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
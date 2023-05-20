using System;

namespace Movies.Api.Models
{
    public class LoginResult
    {
        public string token { get; set; }
        public DateTime expires { get; set; }
    }
}
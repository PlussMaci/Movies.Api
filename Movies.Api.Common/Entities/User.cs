using System;

namespace Movies.Api.Common.Entities
{
    public class User
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}

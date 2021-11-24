﻿using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;

namespace BlogedWebapp.Entities
{
    /// <summary>
    ///  Profile entity
    /// </summary>
    public class Profile : BaseEntity
    {

        [JsonIgnore]
        public IdentityUser Identity { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
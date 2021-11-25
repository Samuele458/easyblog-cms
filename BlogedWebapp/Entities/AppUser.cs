﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogedWebapp.Entities
{
    public class AppUser : IdentityUser
    {
        public Guid? ProfileDataId { get; set; }

        [ForeignKey(nameof(ProfileDataId))]
        [JsonIgnore]
        public ProfileData ProfileData { get; set; }
    }
}

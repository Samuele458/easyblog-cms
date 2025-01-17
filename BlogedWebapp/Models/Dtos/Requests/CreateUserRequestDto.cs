﻿using System.ComponentModel.DataAnnotations;

namespace BlogedWebapp.Models.Dtos.Requests
{
    /// <summary>
    ///  Request Data Transfer Object for creating new user
    /// </summary>
    public class CreateUserRequestDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}

﻿namespace SIS.MvcFramework
{
    using System.ComponentModel.DataAnnotations;

    public class IdentityUser<T>
    {
        public T Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public IdentityRole Role { get; set; }
    }
}

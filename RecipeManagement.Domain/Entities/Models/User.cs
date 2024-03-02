﻿using System.ComponentModel.DataAnnotations;

namespace RecipeManagement.Domain.Entities.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(40)]
        public string Login { get; set; }

        [MinLength(8)]
        public string Password { get; set; }

        [MaxLength(20)]
        public string Role { get; set; }
    }
}
﻿using RecipeManagement.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecipeManagement.Domain.Entities.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [MaxLength(80)]
        public required string Title { get; set; }
        public required List<string> Ingredients { get; set; }
        public required List<string> Instructions { get; set; }
        public required Level DifficultyLevel { get; set; }
        public required List<string> Tags { get; set; }

        [MaxLength(40)]
        public required string Author { get; set; }
        public required short Rating { get; set; }

    }
}

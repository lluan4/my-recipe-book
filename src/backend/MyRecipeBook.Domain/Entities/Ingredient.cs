﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MyRecipeBook.Domain.Entities
{
    [Table("ingredients")]
    public class Ingredient : EntityBase
    {
        public string Item { get; set; } = string.Empty;
        public long RecipeId { get; set; }

    }
}

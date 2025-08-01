﻿using MyRecipeBook.Communication.Enums;

namespace MyRecipeBook.Communication.Request
{
    public class RequestRecipeJson
    {
        public string Title { get; set; } = string.Empty;
        public RecipeCookingTime? CookingTimeId { get; set; }
        public RecipeDifficulty? DifficultyId { get; set; }
        public IList<string> Ingredients { get; set; } = [];
        public IList<RequestInstructionJson> Instructions { get; set; } = [];
        public IList<RecipeDishType> DishTypes { get; set; } = [];
    }
}

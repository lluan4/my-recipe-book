namespace MyRecipeBook.Domain.Dtos.Recipes
{
    public record  GeneratedInstructionDto
    {
        public int Step { get; init; }
        public string Description { get; init; } = string.Empty;
    }
}

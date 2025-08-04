namespace MyRecipeBook.Communication.Response
{
    public class ResponseInstructionJson
    {
        public long Id { get; set; }
        public int Step { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

namespace MyRecipeBook.Application.UseCases.User.Delete
{
	public interface IDeleteUserAccountUseCase
	{
		Task Execute(Guid userIdentifier);
	}
}

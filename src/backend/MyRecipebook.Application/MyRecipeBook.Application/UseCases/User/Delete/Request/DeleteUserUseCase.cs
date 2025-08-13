using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace MyRecipeBook.Application.UseCases.User.Delete.Request
{
	public class DeleteUserUseCase:IDeleteUserUseCase
	{
		private readonly IDeleteUserQueue _queue;
		private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
		private readonly ILoggedUser _loggedUser;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteUserUseCase(IDeleteUserQueue queue, IUserUpdateOnlyRepository userUpdateOnlyRepository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
		{
			_queue = queue;
			_userUpdateOnlyRepository = userUpdateOnlyRepository;
			_loggedUser = loggedUser;
			_unitOfWork = unitOfWork;
		}

		public async Task Execute()
		{
			var loggedUser = await _loggedUser.User();

			var user = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

			user.Active = false;
			
			_userUpdateOnlyRepository.Update(user);

			await _unitOfWork.Commit();

			await _queue.SendMessage(loggedUser);
		}
	}
}

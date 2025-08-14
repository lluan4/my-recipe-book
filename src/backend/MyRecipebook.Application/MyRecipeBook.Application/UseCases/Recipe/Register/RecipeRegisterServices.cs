using AutoMapper;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Application.UseCases.Recipe.Register
{
	public class RecipeRegisterServices
	{
		public ILoggedUser _loggedUser { get; }
		public IUnitOfWork _unitOfWork { get; }
		public IMapper _mapper { get; }
		public IBlobStorageService _blobStorageService { get; }

		public RecipeRegisterServices(ILoggedUser loggedUser, IUnitOfWork unitOfWork, IMapper mapper, IBlobStorageService blobStorageService)
		{
			_loggedUser = loggedUser;
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_blobStorageService = blobStorageService;
		}
	}
}

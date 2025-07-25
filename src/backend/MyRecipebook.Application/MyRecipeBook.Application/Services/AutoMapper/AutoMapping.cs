using AutoMapper;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Request;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Response;
using MyRecipeBook.Communication.Responses;
using Sqids;

namespace MyRecipeBook.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        private readonly SqidsEncoder<long> _idEncoder;
        public AutoMapping(SqidsEncoder<long> idEnconder)
        {
            _idEncoder = idEnconder;

            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<int, Domain.Entities.RecipesDishTypes>()
                .ForMember(dest => dest.DishTypeId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore());

            CreateMap<RequestRecipeJson, Domain.Entities.Recipe>()
                .ForMember(dest => dest.Instructions, opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Ingredients,
                    opt => opt.MapFrom(source => source.Ingredients.Distinct())
                )
                .ForMember(dest => dest.RecipeDishTypes, opt => opt.MapFrom(src => src.DishTypes));

            CreateMap<string, Domain.Entities.Ingredient>()
                .ForMember(dest =>
                    dest.Item, opt => opt.MapFrom(src => src)
                );

            CreateMap<RecipeDishType, Domain.Entities.DishTypes>()
                .ForMember(dest =>
                    dest.Id, opt => opt.MapFrom(src => src)
                );

            CreateMap<RequestInstructionJson, Domain.Entities.Instruction>()
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore());

            CreateMap<RecipeDishType, Domain.Entities.RecipesDishTypes>()
                .ForMember(dest => dest.DishTypeId, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.RecipeId, opt => opt.Ignore())
                .ForMember(dest => dest.Recipe, opt => opt.Ignore())
                .ForMember(dest => dest.DishType, opt => opt.Ignore());
        }

        private void DomainToResponse()
        {
            CreateMap<Domain.Entities.User, ResponseUserProfileJson>();
            CreateMap<Domain.Entities.Recipe, ResponseRegisteredRecipeJson>()
                .ForMember(dest => dest.Id, config => config.MapFrom(src => _idEncoder.Encode(src.Id)));
        }

    }
}

using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.Extension
{
	namespace MyRecipeBook.Domain.Extension
	{
		public static class StreamImageExtension
		{

			public static (bool isValidImage, string imageIdentifier) ValidadeAndGetImageIdentifier(this Stream stream)
			{
				var result = (false, string.Empty);

				if(stream.Is<JointPhotographicExpertsGroup>()) result = (true, $"{Guid.NewGuid()}{NormalizeExtension(JointPhotographicExpertsGroup.TypeExtension)}");

				if(stream.Is<PortableNetworkGraphic>()) result = (true, $"{Guid.NewGuid()}{NormalizeExtension(PortableNetworkGraphic.TypeExtension)}");

				stream.Position = 0;
				
				return result;
			}

			private static string NormalizeExtension (string extension)
			{
				return extension.StartsWith('.') ? extension : $".{extension}";
			}

			public static void ValidateImageFile(this Stream fileString)
			{
				var isInvalidImgType = fileString.Is<PortableNetworkGraphic>().isFalse() && fileString.Is<JointPhotographicExpertsGroup>().isFalse();

				if(isInvalidImgType)
					throw new ErrorOnValidationException([ResourceMessagesException.ONLY_IMAGES_ACCEPTED]);

				fileString.Position = 0;
			}
		}
	}

}

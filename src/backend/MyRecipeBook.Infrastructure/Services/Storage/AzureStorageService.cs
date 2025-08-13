using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extension;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Domain.ValueObjects;

namespace MyRecipeBook.Infrastructure.Services.Storage
{
	public class AzureStorageService:IBlobStorageService
	{
		private readonly BlobServiceClient _blobServiceClient;
		public AzureStorageService(BlobServiceClient blobServiceClient)
		{
			_blobServiceClient = blobServiceClient;
		}

		public async Task Delete(User user, String imageIdentifier)
		{
			var containerClient = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());

			var exists = await containerClient.ExistsAsync();

			if(exists.Value.isFalse()) return;

			await containerClient.DeleteBlobIfExistsAsync(imageIdentifier);
		}

		public async Task<String> GetFileUrl(User user, String imageIdentifier)
		{
			var containerName = user.UserIdentifier.ToString();

			var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

			var exists = await containerClient.ExistsAsync();

			if(exists.Value.isFalse()) return string.Empty;

			var blobClient = containerClient.GetBlobClient(imageIdentifier);

			exists = await blobClient.ExistsAsync();

			if(exists.Value.isFalse()) return string.Empty;

			var sasBuilder = new BlobSasBuilder
			{
				BlobContainerName = containerName,
				BlobName = imageIdentifier,
				Resource = "b",
				ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(MyRecipeBookRuleConstants.MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES),
			};

			sasBuilder.SetPermissions(BlobSasPermissions.Read);

			return blobClient.GenerateSasUri(sasBuilder).ToString();

		}

		public async Task Upload(User user, Stream file, string fileName)
		{
			var container = _blobServiceClient.GetBlobContainerClient(blobContainerName: user.UserIdentifier.ToString());

			await container.CreateIfNotExistsAsync();

			var blobClient = container.GetBlobClient(fileName);

			await blobClient.UploadAsync(file, overwrite: true);
		}

	}
}

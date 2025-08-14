namespace MyRecipeBook.Infrastructure.Migrations
{
	public abstract class DatabaseVersions
	{
		public const int TABLE_USER = 1;
		public const int TABLE_RECIPES = 2;
		public const int SEED_ADMIN_DATA = 3;
		public const int IMAGES_FOR_RECIPES = 4;
		public const int TABLE_REFRESH_TOKEN = 5;
	}
}

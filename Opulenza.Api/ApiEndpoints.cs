namespace Opulenza.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    public static class Authentication
    {
        private const string Base = $"{ApiBase}/auth";

        public const string Login = $"{Base}/login";
        public const string Register = $"{Base}/register";
        public const string RefreshToken = $"{Base}/refresh-token";
        public const string RevokeToken = $"{Base}/revoke-token";
        public const string ChangePassword = $"{Base}/change-password";
        public const string ConfirmEmail = $"{Base}/confirm-email";
        public const string RequestResetPassword = $"{Base}/request-reset-password";
        public const string ResetPassword = $"{Base}/reset-password";
        public const string LoginWithGitHub = $"{Base}/login-github";
        public const string LoginWithGitHubCallback = $"{Base}/login-github-callback";
        public const string DeleteAccount = $"{Base}/delete-account";
    }

    public static class Users
    {
        private const string Base = $"{ApiBase}/users/me";

        public const string GetUser = $"{Base}";

        public const string UploadImage = $"{Base}/image";
        public const string PublicImage = $"{Base}/image";

        public const string GetUserAddress = $"{Base}/address";
        public const string UpdateAddress = $"{Base}/address";
    }

    public static class Products
    {
        private const string Base = $"{ApiBase}/products";
        
        public const string GetProducts = $"{Base}";
        public const string GetProductById = $"{Base}/{{id:int}}";
        public const string GetProductBySlug = $"{Base}/{{slug}}";
        public const string AddProduct = $"{Base}";
        public const string UpdateProduct = $"{Base}/{{id}}";
        public const string DeleteProduct = $"{Base}/{{id}}";

        public static class Images
        {
            private const string ImagesBase = $"{ApiBase}/products/{{id}}/images";
            
            public const string GetImages = $"{ImagesBase}";
            public const string AddImages = $"{ImagesBase}";
            public const string DeleteImage = $"{ImagesBase}/{{imageId}}";
        }

        public static class Ratings
        {
            private const string RatingsBase = $"{ApiBase}/products/{{id}}/ratings";
            
            public const string GetRatings = $"{RatingsBase}";
            public const string AddRating = $"{RatingsBase}";
            public const string UpdateRating = $"{RatingsBase}/{{ratingId}}";
            public const string DeleteRating = $"{RatingsBase}/{{ratingId}}";
            
        }
        
        public static class Categories 
        {
            private const string CategoriesBase = $"{ApiBase}/products/{{id}}/categories";
            
            public const string GetCategories = $"{CategoriesBase}";
            public const string AddCategory = $"{CategoriesBase}";
            public const string DeleteCategory = $"{CategoriesBase}/{{categoryId}}";
        }
    
    }
}
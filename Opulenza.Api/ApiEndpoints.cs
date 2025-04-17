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

        // Admin 
        public const string GetUsers = $"{ApiBase}/users";
        public const string BlockUser = $"users/block/{{id}}";
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
            public const string AddCategories = $"{CategoriesBase}";
            public const string UpdateCategories = $"{CategoriesBase}";
            public const string DeleteCategories = $"{CategoriesBase}";
        }
    }

    public static class Categories
    {
        // no circular dependency 
        private const string Base = $"{ApiBase}/categories";
        public const string GetCategories = $"{Base}";
        public const string AddCategory = $"{Base}";
        public const string UpdateCategory = $"{Base}/{{id}}";
        public const string DeleteCategory = $"{Base}/{{id}}";

        public static class Images
        {
            private const string ImagesBase = $"{ApiBase}/categories/{{id}}/images";
            public const string GetImages = $"{ImagesBase}";
            public const string AddImages = $"{ImagesBase}";
            public const string DeleteImage = $"{ImagesBase}/{{imageId}}";
        }
    }

    public static class Carts
    {
        private const string Base = $"{ApiBase}/carts";
        public const string GetCart = $"{Base}";
        public const string UpdateCart = $"{Base}";
    }

    public static class Wishlists
    {
        private const string Base = $"{ApiBase}/wishlists";
        public const string GetWishlist = $"{Base}";
        public const string AddToWishlist = $"{Base}";
        public const string RemoveFromWishlist = $"{Base}/{{id}}";
    }

    public static class Orders
    {
        private const string Base = $"{ApiBase}/orders";

        public const string GetOrders = $"{Base}";
        public const string GetOrder = $"{Base}/{{id}}";
        public const string Checkout = $"{Base}/checkout";
        public const string Webhook = $"{Base}/webhook";
    }
}
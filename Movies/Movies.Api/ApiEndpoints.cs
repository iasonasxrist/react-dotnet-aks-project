namespace Movies.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "Api";

    public static class Movies
    {
        private const string Base = $"{ApiBase}/movies";

        public const string Create = Base;
        public const string Get = $"{Base}/{{id:guid}}";
        public const string GetAll = $"{Base}/GetAll";
        public const string Update = $"{Base}/{{id:guid}}";
        public const string Delete = $"{Base}/{{id:guid}}";



        
    }

}
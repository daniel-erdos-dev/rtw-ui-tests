using OpenQA.Selenium;

namespace RTW_UI_TESTS
{
    public static class CommonSelectors
    {
        // HOME PAGE
        public static readonly By FavoriteMovieInputSelector = By.Id("home-favoriteMovieInput");
        public static readonly By RecommendButtonSelector = By.Id("home-recommendButton");

        // MOVIE CARD
        public static readonly By MovieTitleSelector = By.ClassName("movie-title");

        public static readonly By MovieYearSelector = By.ClassName("movie-year");

        public static readonly By MoviePosterSelector = By.ClassName("movie-poster");

        public static readonly By MovieOverviewSelector = By.ClassName("movie-overview");
    }
}
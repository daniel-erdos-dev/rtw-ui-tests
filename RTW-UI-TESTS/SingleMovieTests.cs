using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace RTW_UI_TESTS;

[TestFixture]
[Description("UI tests for single movie option based on favorite movie")]
public class SingleMovieTests
{
    private IWebDriver driver;
    private readonly string currentMovieTitle = "Eternal Sunshine of the Spotless Mind";

    [SetUp]
    public void Setup()
    {
        driver = Environment.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            _ => new ChromeDriver(),
        };

        driver.Url = Environment.BaseUrl;
        driver.Manage().Window.FullScreen();

        IWebElement recommendButton = driver.FindElement(CommonSelectors.RecommendButtonSelector);
        IWebElement favoriteMovieInput = driver.FindElement(CommonSelectors.FavoriteMovieInputSelector);

        SeleniumHelpers.WaitUntil(() => favoriteMovieInput.Displayed && favoriteMovieInput.Enabled, 3);

        favoriteMovieInput.SendKeys(currentMovieTitle);

        SeleniumHelpers.WaitUntil(() => favoriteMovieInput.GetDomProperty("value") == currentMovieTitle, 3);

        SeleniumHelpers.WaitUntil(() => recommendButton.Enabled, 3);
        recommendButton.Click();

        SeleniumHelpers.WaitUntil(() => driver.Url.Contains("selectedMovie"), 3);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }

    [Test]
    [Description("Necessary UI element should be visible for the relevant single movie")]
    public void BasicUiForSingleMoviePage()
    {
        IWebElement headerText = driver.FindElement(By.Id("singleMovie-headerText"));
        IWebElement movieTitle = driver.FindElement(CommonSelectors.MovieTitleSelector);
        IWebElement movieYear = driver.FindElement(CommonSelectors.MovieYearSelector);
        IWebElement moviePoster = driver.FindElement(CommonSelectors.MoviePosterSelector);
        IWebElement movieDescription = driver.FindElement(CommonSelectors.MovieOverviewSelector);
        IWebElement yesButton = driver.FindElement(By.Id("singleMovie-yesButton"));
        IWebElement noButton = driver.FindElement(By.Id("singleMovie-noButton"));

        List<IWebElement> singleMovieElements = [headerText, movieTitle, movieYear, moviePoster, movieDescription, yesButton, noButton];
        
        // Assertions
        singleMovieElements.AllElementsAreVisible();
        Assert.Multiple(() => {
            Assert.That(headerText.Text, Is.EqualTo("Is this the movie you thought about?"));
            Assert.That(movieTitle.Text, Is.EqualTo(currentMovieTitle));
            Assert.That(movieYear.Text, Is.EqualTo("(2004)"));
            Assert.That(moviePoster.GetDomProperty("alt"), Is.EqualTo("Poster for Eternal Sunshine of the Spotless Mind"));
        });
    }

    [Test]
    [Description("Upon clicking NO button on the single movie page it should redirect to the home page")]
    public void SingleMovieClickNoRedirectHome()
    {
        IWebElement noButton = driver.FindElement(By.Id("singleMovie-noButton"));

        Assert.That(noButton.Displayed && noButton.Enabled, Is.True);

        noButton.Click();

        CommonChecks.WeAreOnHomePage(driver);
    }

    [Test]
    [Description("Upon clicking YES button on the single movie page it should redirect to the recommendations page")]
    public void SingleMovieClickYesRedirectRecommendations()
    {
        IWebElement yesButton = driver.FindElement(By.Id("singleMovie-yesButton"));

        Assert.That(yesButton.Displayed && yesButton.Enabled, Is.True);

        yesButton.Click();

        CommonChecks.WeAreOnRecommendedMoviesPage(driver);
    }
}
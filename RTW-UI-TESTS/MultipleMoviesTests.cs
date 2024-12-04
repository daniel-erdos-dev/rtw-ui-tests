using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace RTW_UI_TESTS;

[TestFixture]
[Description("UI tests for multiple movie options based on favorite movie given")]
public class MultipleMoviesTests
{
    private IWebDriver driver;

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

        favoriteMovieInput.SendKeys("Godfather");

        SeleniumHelpers.WaitUntil(() => favoriteMovieInput.GetDomProperty("value") == "Godfather", 3);

        SeleniumHelpers.WaitUntil(() => recommendButton.Enabled, 3);
        recommendButton.Click();

        SeleniumHelpers.WaitUntil(() => driver.Url.Contains("selectedMovies"), 3);
    }

    [TearDown]
    public void TearDown()
    {
        driver.Close();
        driver.Quit();
    }

    [Test]
    [Description("Necessary UI element should be visible for the relevant single movie")]
    public void BasicUiElementsTestforMultipleMovieSelections()
    {
        
        List<IWebElement> movieTitles = [.. driver.FindElements(CommonSelectors.MovieTitleSelector)];
        List<IWebElement> movieYears = [.. driver.FindElements(CommonSelectors.MovieYearSelector)];
        List<IWebElement> moviePosters = [.. driver.FindElements(CommonSelectors.MoviePosterSelector)];
        List<IWebElement> movieDescriptions = [.. driver.FindElements(CommonSelectors.MovieOverviewSelector)];
        List<IWebElement> chooseMovieButtons = [.. driver.FindElements(By.ClassName("multipleMovies-chooseButton"))];

        Assert.Multiple(() => {
            Assert.That(movieTitles, Has.Count.EqualTo(movieYears.Count));
            Assert.That(movieYears, Has.Count.EqualTo(moviePosters.Count));
            Assert.That(moviePosters, Has.Count.EqualTo(movieDescriptions.Count));
            Assert.That(movieDescriptions, Has.Count.EqualTo(chooseMovieButtons.Count));
            Assert.Multiple(() => {
                chooseMovieButtons.ForEach(button => Assert.That(button.Enabled, Is.True));
            });
        });

    }

    [Test]
    [Description("Necessary UI element should be visible for the relevant single movie")]
    public void MultipleMoviesClickingChooseMovieRedirectsToRecommendationsPage()
    {
        List<IWebElement> chooseMovieButtons = [.. driver.FindElements(By.ClassName("multipleMovies-chooseButton"))];

        chooseMovieButtons.First().Click();

        CommonChecks.WeAreOnRecommendedMoviesPage(driver);
    }
}

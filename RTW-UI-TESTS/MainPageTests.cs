using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace RTW_UI_TESTS;

[TestFixture]
[Description("UI tests for the home page of RTW")]
public class MainPageTests
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    private readonly By RecommendButtonSelector = By.Id("home-recommendButton");
    private readonly By HeaderTextSelector = By.Id("home-headerText");
    private readonly By FavoriteMovieInputSelector = By.Id("home-favoriteMovieInput");

    public MainPageTests() {
        driver = Environment.BrowserType switch
        {
            BrowserType.Chrome => new ChromeDriver(),
            BrowserType.Firefox => new FirefoxDriver(),
            _ => new ChromeDriver(),
        };

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
    }


    [SetUp]
    public void SetupMainPageTests()
    {
        driver.Url = "http://localhost:3000/";
        driver.Manage().Window.FullScreen();
    }

    [TearDown]
    public void TearDownMainPageTests()
    {
        driver.Close();
        driver.Quit();
    }

    [Test]
    [Description("Home page should contain header text, input field and recommend button")]
    public void TestBasicUIElements()
    {
        IWebElement recommendButton = driver.FindElement(RecommendButtonSelector);
        IWebElement headerText = driver.FindElement(HeaderTextSelector);
        IWebElement favoriteMovieInput = driver.FindElement(FavoriteMovieInputSelector);

        Assert.Multiple(() => 
        {
            Assert.That(recommendButton.Displayed && !recommendButton.Enabled, Is.True);
            Assert.That(headerText.Displayed, Is.True);
            Assert.That(favoriteMovieInput.Displayed && favoriteMovieInput.Enabled, Is.True);

            Assert.That(recommendButton.Text, Is.EqualTo("Recommend"));
            Assert.That(headerText.Text, Is.EqualTo("Search for your favorite movie and get movie recommendations based on it!"));
        });
    }

    [Test]
    [Description("Recommend button should be enabled when there is text in the input field")]
    public void TestRecommendButtonStateChange()
    {
        IWebElement recommendButton = driver.FindElement(RecommendButtonSelector);
        IWebElement favoriteMovieInput = driver.FindElement(FavoriteMovieInputSelector);

        // base state
        Assert.Multiple(() => 
        {
            Assert.That(recommendButton.Displayed && !recommendButton.Enabled, Is.True);
            Assert.That(favoriteMovieInput.Displayed && favoriteMovieInput.Enabled, Is.True);
        });

        favoriteMovieInput.SendKeys("Se7en");

        SeleniumHelpers.WaitUntil(() => favoriteMovieInput.GetDomProperty("value") == "Se7en", 5);
        // changed state
        Assert.That(recommendButton.Enabled, Is.True);
        
    }
}

using OpenQA.Selenium;

namespace RTW_UI_TESTS;

public static class CommonChecks
{
    public static void WeAreOnHomePage(IWebDriver driver)
    {
        SeleniumHelpers.WaitUntil(() => driver.Url == Environment.BaseUrl, 3);
        Assert.That(driver.Url, Is.EqualTo(Environment.BaseUrl));
        IWebElement recommendButton = driver.FindElement(By.Id("home-recommendButton"));
        Assert.That(recommendButton.Displayed, Is.True);
    }

    public static void WeAreOnRecommendedMoviesPage(IWebDriver driver)
    {
        SeleniumHelpers.WaitUntil(() => driver.Url.Contains("recommendedMovies"), 8);
        driver.WaitUntilNotDisplayed(By.ClassName("loader"), 10);

        SeleniumHelpers.SafeIsDisplayed(driver, By.ClassName("recommendedMovie-whereToWatchButton"), 10);
        
        List<IWebElement> whereToWatchButtons = [.. driver.FindElements(By.ClassName("recommendedMovie-whereToWatchButton"))];

        Assert.That(whereToWatchButtons, Is.Not.Null);
        Assert.That(whereToWatchButtons, Is.Not.Empty);
    }
}
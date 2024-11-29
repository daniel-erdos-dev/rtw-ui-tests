using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace RTW_UI_TESTS
{
    public static class SeleniumHelpers
    {
       public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                WebDriverWait wait = new(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static void WaitUntil(Func<bool> predicate, int timeoutInSeconds)
        {
            if (timeoutInSeconds <= 0)
            {
                throw new ArgumentException("The timeout must be a positive, non-zero value");
            }
            
            DateTime timeoutMoment = DateTime.Now + TimeSpan.FromSeconds(timeoutInSeconds);

            int interval = 1;
            Exception? lastException = null;

            do
            {
                try
                {
                    if (predicate())
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lastException = ex;
                }

                Thread.Sleep(interval);
                interval = Math.Min(interval * 2, 100);

            } while (DateTime.Now < timeoutMoment);

            if (lastException != null)
            {
                throw lastException;
            }
            else
            {
                throw new TimeoutException();
            }
        }

    }
}

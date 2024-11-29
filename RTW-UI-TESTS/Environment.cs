namespace RTW_UI_TESTS
{
    public static class Environment
    {
        public static BrowserType BrowserType { get; set; } = BrowserType.Chrome;
        public static string BaseUrl { get; } = "https://witty-beach-04f598103.5.azurestaticapps.net/";
        public static string AboutUrl { get; } = "https://witty-beach-04f598103.5.azurestaticapps.net/about";
    }
}

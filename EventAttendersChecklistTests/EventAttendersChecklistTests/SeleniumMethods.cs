using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace EventAttendersChecklistTests
{
	class SeleniumMethods
	{
		public static IWebDriver ConfigureWebDriver(string path)
		{
			IWebDriver driver = new InternetExplorerDriver(path);
			driver.Manage().Window.Maximize();

			return driver;
		}
	}
}

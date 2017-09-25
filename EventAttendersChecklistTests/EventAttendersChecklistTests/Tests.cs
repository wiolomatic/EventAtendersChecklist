using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace EventAttendersChecklistTests
{
	[TestClass]
	public class Tests
	{
		private IWebDriver driver;

		[TestInitialize()]
		public void TestInitialize()
		{
			driver = SeleniumMethods.ConfigureWebDriver(SeleniumParameters.IeDriverPath);
		}

		[TestCleanup()]
		public void TestCleanup()
		{
			driver.Dispose();
		}

		[TestMethod]
		public void CheckDriverSetup()
		{
			Assert.IsTrue(driver != null);
		}
	}
}

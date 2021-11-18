using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jayride_Website_Selenium_Tests
{
    [TestClass]
    public class Jayride_Website_Selenium_Tests
    {

        IWebDriver? _driver;

        [TestInitialize]
        public void Setup()
        {
            String website_url = "https://www.jayride.com";
            
        }

        [TestCleanup]
        public void Teardown()
        {
            // close the browser
            _driver.Close();
        }

        /// <summary>
        /// Test Case: Request Quotes for airport transfers from Sydney Airport (SYD), valid date, including return, for 1 Passenger. 
        /// Given: A user navigates to the Jayride.com website
        /// When: The user searches for a transfer in the SYD market & validates the results & price.
        /// Then: All information should be correct
        /// </summary>
        [TestMethod]
        public void Jayride_Website_Quote_Transfer_SYD_1PAX()
        {
            Console.Write("Jayride_Website_001");

            #region Test Data

            string fromLocation = "Sydney Airport (SYD), T1 International Terminal";
            string toLocation = "46 Church St, Parramatta NSW 2150, Australia";
            string bookingDate = "Tuesday, 30 Nov, 2021";
            string bookingTime = "17:10 (5:10 PM)";
            string jayrideTitle_1 = "Jayride.com | Compare and Book Airport Shuttles and Private Transfers";
            string selectedTransfer = "GoCatch Standard";
            string selectedTransferPrice = "AU$ 113.00";

            #endregion Test Data

            #region initialization

            // initialize the driver to be used 
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            // navigate to URL  
            _driver.Navigate().GoToUrl("https://www.jayride.com/");

            // assertion variable
            String ActualTitle = _driver.Title;
            String ExpectedTitle = jayrideTitle_1;

            #endregion initialization

            #region Fill out search details 

            // populate "from location"
            IWebElement from_location = _driver.FindElement(By.Id("from_location"));
            from_location.SendKeys(fromLocation);
            waitUntilElementIsClickable("//*[@id=\"ui-id-3\"]/div", 10);
            from_location.SendKeys(Keys.ArrowDown);
            from_location.SendKeys(Keys.Tab);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // populate "to location"
            IWebElement to_location = _driver.FindElement(By.Id("to_location"));
            to_location.SendKeys(toLocation);
            waitUntilElementIsClickable("//*[@id=\"ui-id-13\"]/div", 10);
            to_location.SendKeys(Keys.ArrowDown);
            to_location.SendKeys(Keys.Tab);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // populate "outbound date"
            IWebElement outbound_date_input = _driver.FindElement(By.Id("outbound_date_input"));
            outbound_date_input.Click();

            IWebElement outbound_date = _driver.FindElement(By.XPath("//*[@id=\"ui-datepicker-div\"]/table/tbody/tr[5]/td[3]/a"));
            outbound_date.Click();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Keep passengers to 1 PAX
            IWebElement passenger_select = _driver.FindElement(By.Id("passenger_select"));
            passenger_select.Click();

            waitUntilElementIsClickable("//button[contains(@onclick,'passenger_select')]", 5);
            IWebElement increase = _driver.FindElement(By.XPath("//button[contains(@onclick,'passenger_select')]"));
            increase.Click();

            // click on search button
            IWebElement compare_button = _driver.FindElement(By.Id("compare_button"));
            compare_button.Click();

            #endregion Fill out search details 

            // adding a sleep here as a potential bug is found with UI when to_location is populated with Selenium Webdriver, returns an error the first time the submit button is pressed.
            Thread.Sleep(4000);
            // click on submit
            compare_button.Click();

            #region Get quote page details
            // wait for page to be loaded
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            IWebElement quote_from = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[1]/div[1]/div/jr-location-autocomplete/div/div/div/input")));
            IWebElement quote_to = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/app-root/jr-layout/div/div/div/jr-find-page/div/div[2]/jr-search-box/div/div/div/div/div[1]/div[2]/div/jr-location-autocomplete/div/div/div/input")));

            string numberOfResults = _driver.FindElement(By.XPath("//*[@id=\"search-results\"]/div/div/div/div[1]/div[1]")).Text;

            
            // get values from quote page for assertion
            string quote_from_text = quote_from.GetAttribute("value");
            string quote_to_text = quote_to.GetAttribute("value");

            #endregion Get quote page details

            #region Booking Details

            // select the desired transfer
            IWebElement select_transfer = _driver.FindElement(By.XPath("//*[@id=\"search-results\"]/div/div/div/div[2]/jr-quote-group[1]/div/div[1]/jr-quote-card[1]/div/div/div/div/div/div[2]/div[2]/div/div[2]/button"));
            select_transfer.Click();

            // element located in booking-details page
            waitUntilElementIsClickable("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[1]/div[1]/jr-quote-card/div/div/div/div/div/div[2]/div[2]/div/div/button", 10);

            string details_from = _driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[2]/div/jr-transfer-details/div/div/div/div[1]/div[1]/dl/dd")).Text;
            string details_to = _driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[2]/div/jr-transfer-details/div/div/div/div[1]/div[2]/dl/dd")).Text;
            string details_date = _driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[2]/div/jr-transfer-details/div/div/div/div[3]/div[1]/dl/dd")).Text;
            string details_time = _driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[2]/div/jr-transfer-details/div/div/div/div[3]/div[2]/dl/dd")).Text;
            string details_selectedTransfer = _driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[1]/div[1]/jr-quote-card/div/div/div/div/div/div[1]/div[1]/div/div[1]/h4")).Text;
            string details_price_selectedTransfer = _driver.FindElement(By.XPath("/html/body/app-root/jr-layout/div/div/div/ng-component/div/div[1]/div[1]/div[1]/jr-quote-card/div/div/div/div/div/div[2]/div[1]/jr-quote-price/div/div")).Text;

            #endregion Booking Details

            #region Assertions

            // check if the script started on the correct page.
            Assert.AreEqual(ExpectedTitle, ActualTitle);

            //[Quote]
            // check if quote details returned are from values inputted in jayride.com page.
            Assert.AreEqual(fromLocation, quote_from_text);
            Assert.AreEqual(toLocation, quote_to_text);

            //[Booking Details]
            Assert.AreEqual(fromLocation, details_from);
            Assert.AreEqual(toLocation, details_to);
            Assert.AreEqual(bookingDate, details_date);
            Assert.AreEqual(bookingTime, details_time);
            Assert.AreEqual(selectedTransfer, details_selectedTransfer);
            Assert.AreEqual(selectedTransferPrice, details_price_selectedTransfer);
            //Pricing

            #endregion Assertions

        }

        /// <summary>
        /// Validates if a specific element is visible & waits before proceeding
        /// </summary>
        /// <param name="elementXpath">
        /// The XPath (string) of the element to be validated/
        /// </param>
        public void waitUntilElementIsClickable(string elementXpath, int waitTimeInSeconds)
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(waitTimeInSeconds));
            IWebElement element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(elementXpath)));
        }
    }
}
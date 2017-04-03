//********************************************************
//** Test project developed by WL Tsang        ***********
//** to get pressure from the BBC weather page ***********
//********************************************************

using System;
using System.Text;
using System.Threading;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace weather
{
    [TestFixture]
    public class weather
    {
        private StringBuilder verificationErrors;
        public IWebDriver driver;
        public Common_Elements page;
        public string baseURL = Properties.Settings.Default.URL;
        public string location = Properties.Settings.Default.location;
        public string specifiedHour = Properties.Settings.Default.specifiedHour; 

        [SetUp]
        public void SetupTest()
        {
            //driver = new InternetExplorerDriver();
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait =TimeSpan.FromSeconds(5); 
            verificationErrors = new StringBuilder();
            page = new Common_Elements(driver);
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                if (driver != null)
                    driver.Quit();
            }
            catch (Exception ex)
            {
                verificationErrors.Append(ex.Message);
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void read_pressure()
        {
            //************ ******************
            string today_pressure;
            string tomorrow_pressure;
            int diff_pressure;
            Boolean IsDateAfterTomorrow = false;

            driver.Navigate().GoToUrl(baseURL);
            //driver.Manage().Window.Maximize();
            //** Verify its BBC weather page. Type Reading and search
            Assert.AreEqual("BBC Weather", driver.Title);
            IsElementPresent(page.lblTitle);
            page.txtlocation.Click();
            page.txtlocation.SendKeys(location);
            page.icnSearch.Click();
            Wait_For_Element(page.linkReading, 3);
            page.linkReading.Click();
            IsElementPresent(page.lblReading);
            page.icnShowTable.Click();

            //** Find position of "2100" hour on table. If not found, use tomorrow's table
            int hr_position = page.FindHourPosition(specifiedHour);
            if (hr_position == -1 )
            {
                //** Hour 21 not found in today page, so use value of tomorrow and date after tomorrow
                page.icnTomorrow.Click();
                Thread.Sleep(2000);
                hr_position = page.FindHourPosition(specifiedHour);
                IsDateAfterTomorrow = true;
            }

            //** Get today/tomorrow Pressure reading based on hour position
            today_pressure = page.ReadPressure(hr_position);

            //** Get tomorrow or day3 Pressure
            if (IsDateAfterTomorrow)
            {
                page.icnDay3.Click();
            }
            else
            {
                page.icnTomorrow.Click();
            }
            Thread.Sleep(2000);
            hr_position = page.FindHourPosition(specifiedHour);
            tomorrow_pressure = page.ReadPressure(hr_position);

            //** Write results to console output and file (c:\temp\recordedPressure.txt)
            diff_pressure = Int32.Parse(tomorrow_pressure) - Int32.Parse(today_pressure);
            string result = DateTime.Now.ToString() + "\n";
            result += " Today Pressure = " + today_pressure + "\n";
            result += " Tomorrow Pressure = " + tomorrow_pressure + "\n";
            result += " Diff Pressure = " + diff_pressure + "\n";
            Console.WriteLine(result);
            WriteToFile(result);

            driver.Close();

        }

        public void IsElementPresent(IWebElement element)
        {
            try
            {
                if (element.Displayed)
                    return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
            Assert.Fail("Element not displayed");
        }

        public void Wait_For_Element(IWebElement element, int waittime_sec)
        {
            try
            {
                for (int second = 0; ; second++)
                {
                    if (second >= waittime_sec) Assert.Fail("timeout ");
                    try
                    {
                        if (element.Displayed) break;
                    }
                    catch (Exception ex)
                    {
                    }
                    Thread.Sleep(1000);
                }
            }
            catch (AssertionException e)
            {
                verificationErrors.Append(e.Message);
            }
        }

        public void WriteToFile(string message)
        {
            Directory.CreateDirectory(@"C:\temp");
            var filename = @"C:\temp\recordedPressure.txt";
            using (StreamWriter _file = File.AppendText(filename))
            {
                _file.WriteLine(message);
                _file.Close();
            }
        }
    }
}
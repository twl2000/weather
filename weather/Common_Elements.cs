using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace weather
{
    public class Common_Elements
    {
        //Constructor required;
        public Common_Elements(IWebDriver driver)
        {
            PageFactory.InitElements(driver, this);
        }

        //************************ Elements on page ******************************
        //*** Main Page Title
        [FindsBy(How = How.XPath, Using = "//a[@class='site-name']")]
        public IWebElement lblTitle { get; set; }

        //** Input location field
        [FindsBy(How = How.Id, Using = "locator-form-search")]
        public IWebElement txtlocation { get; set; }

        //** Search submit icon
        [FindsBy(How = How.Id, Using = "locator-form-submit")]
        public IWebElement icnSearch { get; set; }

        //** Reading link
        [FindsBy(How = How.XPath, Using = "//a[@href='/weather/2639577']")]
        public IWebElement linkReading { get; set; }

        //** Weather page Reading title
        [FindsBy(How = How.XPath, Using = "//span[(@class='location-name')]")]
        public IWebElement lblReading { get; set; }

        //** show table icon
        [FindsBy(How = How.Id, Using = "detail-table-view")]
        public IWebElement icnShowTable { get; set; }

        //** weather info table
        [FindsBy(How = How.XPath, Using = "//div[contains(@class, 'table-container selected slot-offset')]")]
        public IWebElement tblWeather { get; set; }
        
        //** Date icon - tomorrow
        [FindsBy(How = How.XPath, Using = "//a[@href='/weather/2639577?day=1']")]
        public IWebElement icnTomorrow { get; set; }

        //** Date3 icon - dateAfterTomorrow
        [FindsBy(How = How.XPath, Using = "//a[@href='/weather/2639577?day=2']")]
        public IWebElement icnDay3 { get; set; }

        //** Find the column position containing specified hour i.e. 2100
        public int FindHourPosition(string specifiedHour)
        {
            ICollection<IWebElement> rows = tblWeather.FindElements(By.XPath("//tr[@class='time']/th[contains(@class, 'value hours')]"));

            int position = 0;
            foreach (var row in rows)
            {
                if (row.Text == specifiedHour)
                {
                    return position;
                }
                position = position + 1;
            }
            return -1;
        }

        //** Read Pressure of column with hour = 2100
        public string ReadPressure(int hr_position)
        {
            ICollection<IWebElement> rows = tblWeather.FindElements(By.XPath("//tr[@class='pressure']/td[contains(@class, 'value hours')]"));

            int position = 0;
            foreach (var row in rows)
            {
                if (position == hr_position)
                {
                    return row.Text;
                }
                position = position + 1;
            }
            return "null";
        }

        //** Read Temperature of column with hour = 2100
        public string ReadTemperature(int hr_position)
        {
            ICollection<IWebElement> rows = tblWeather.FindElements(By.XPath("//tr[@class='temperature']/td[contains(@class, 'value hours')]"));

            int position = 0;
            foreach (var row in rows)
            {
                if (position == hr_position)
                {
                    IWebElement temp = row.FindElement(By.XPath("//span[@class='content temp-range temp-10-12']/span[@class='units-values temperature-units-values']/span[@class='units-value temperature-value temperature-value-unit-c']"));
                    return temp.Text;
                }
                position = position + 1;
            }
            return "null";
        }

    }
}

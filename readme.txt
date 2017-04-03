Test script developed to test BBC weather AUT, based on requirement.
====================================================

Notes:
1) Project "weather" created using visual studio 2015 and .Net 4.5.2

2) Nuget is used to install the following packages
* NUnit 
* NUnit3TestAdaptor
* Selenium.WebDriver
* Selenium.Support
* Selenium.WebDriver.ChromeDriver
* Selenium.Firefox.WebDriver
* Selenium.WebDriver.IEDriver

3) Implementation on 2 C# files and 1 config file
i - Common_Element.cs: This contains DOM elements identified on the web portal. These elements are used by the test.
ii - weather.cs: This contains the actual test and test logic. 
iii - app.config: This contains settings for URL, location and specified hour (i.e. 2100)

4) Results (pressure retrieved from the bbc web site and difference)  are 
i - sent to the console output
ii - saved in file C:\temp\recordedPressure.txt

5) To execute
i - open solution using visual studio
ii - build the solution
iii - open Test Explorer and a test named "read_pressure" should be found.
iv - right click to run 
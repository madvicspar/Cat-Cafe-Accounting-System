using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TestProject_For_UI
{
    public class Tests
    {
        public const string DriverUrl = "http://127.0.0.1:4723/";
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            // run WPF exe
            string appWPF = @"D:\vikto\source\repos\Cats-Cafe-Accounting-System\Cats-Cafe-Accounting-System\bin\Debug\net6.0-windows\Cats-Cafe-Accounting-System.exe";
            var options = new AppiumOptions();
            options.AddAdditionalCapability("app", appWPF);
            var session = new WindowsDriver<WindowsElement>(new Uri(DriverUrl), options);

            // wait running WPF (3 seconds)
            Thread.Sleep(1000);

            WindowsElement element = null;
            ReadOnlyCollection<string> windowHandlesBefore = session.WindowHandles;


            // autotest write in wpf application text "dog"
            element = session.FindElementByAccessibilityId("LoginTextId");
            element.SendKeys("admin");

            // autotest click button in wpf application
            element = session.FindElementByAccessibilityId("PassPassId");
            element.SendKeys("admin");

            // autotest get text from wpf application
            element = session.FindElementByAccessibilityId("SignInButtonId");
            element.Click();

            ReadOnlyCollection<string> windowHandlesAfter = session.WindowHandles;

            //element = session.FindElementByAccessibilityId("CloseButtonId");

            // result
            Assert.AreNotEqual(windowHandlesBefore, windowHandlesAfter);

            // close wpf application
            //session.Dispose();
            session.Quit();
            //Process.GetCurrentProcess().Kill();
        }
    }
}
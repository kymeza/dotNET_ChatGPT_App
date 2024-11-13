using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace SecurityTests.Steps;

[Binding]
public class LoginSteps
{
    private IWebDriver _driver;

    [BeforeScenario]
    public void Setup()
    {
        _driver = new ChromeDriver();
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }

    [AfterScenario]
    public void TearDown()
    {
        _driver.Quit();
    }

    [Given(@"the user navigates to ""(.*)""")]
    public void GivenTheUserNavigatesTo(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    [When(@"the user enters ""(.*)"" into the ""(.*)"" field")]
    public void WhenTheUserEntersIntoTheField(string text, string fieldName)
    {
        var field = _driver.FindElement(By.Name(fieldName));
        field.SendKeys(text);
    }

    [When(@"the user clicks the ""(.*)"" button")]
    public void WhenTheUserClicksTheButton(string buttonText)
    {
        var button = _driver.FindElement(By.XPath($"//button[text()='{buttonText}']"));
        button.Click();
    }

    [Then(@"the user should see ""(.*)"" on the page")]
    public void ThenTheUserShouldSeeOnThePage(string expectedText)
    {
        var bodyText = _driver.FindElement(By.TagName("body")).Text;
        Assert.That(bodyText.Contains(expectedText), $"Expected text '{expectedText}' not found on the page.");
    }
}
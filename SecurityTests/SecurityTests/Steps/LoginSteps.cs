using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Reqnroll;

namespace SecurityTests.Steps;

[Binding]
public class LoginSteps
{

    private IWebDriver _driver;

    [BeforeScenario]
    public void SetUp()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--ignore-certificate-errors");


        _driver = new ChromeDriver(chromeOptions);
    }

    [AfterScenario]
    public void TearDown()
    {
        //_driver.Quit();
    }
    

    [Given(@"Un usuario entra a la url ""(.*)""")]
    public void GivenUnUsuarioEntraALaUrl(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    [When(@"el usuario ingresa ""(.*)"" en el campo de username")]
    public void WhenElUsuarioIngresaStringEnElCampoUsername(string userName)
    {
        var usernameInput = _driver.FindElement(By.Id("username"));
        usernameInput.SendKeys(userName);
    }

    [When(@"el usuario ingresa ""(.*)"" en el campo de password")]
    public void WhenElUsuarioIngresaEnElCampoDePassword(string password)
    {
        var passwordInput = _driver.FindElement(By.XPath("/html/body/app-root/app-login/div/form/input[2]"));
        passwordInput.SendKeys(password);
    }

    [When("el usuario presiona el boton login")]
    public void WhenElUsuarioPresionaElBotonLogin()
    {
        var botonLogin = _driver.FindElement(By.XPath("/html/body/app-root/app-login/div/form/button"));
        botonLogin.Click();
    }

    [Then(@"el usuario es redireccionado a ""(.*)""")]
    public void ThenElUsuarioEsRedireccionadoA(string urlEsperada)
    {
        var urlActual = _driver.Url;

        var estoyEnLaUrlDeseada = urlActual.Contains("/chat");

        Assert.That(estoyEnLaUrlDeseada);

    }


}
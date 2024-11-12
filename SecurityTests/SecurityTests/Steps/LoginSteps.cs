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

    }

    [When(@"el usuario ingresa ""(.*)"" en el campo de password")]
    public void WhenElUsuarioIngresaEnElCampoDePassword(string p0)
    {
        throw new PendingStepException();
    }

    [When("el usuario presiona el boton login")]
    public void WhenElUsuarioPresionaElBotonLogin()
    {
        throw new PendingStepException();
    }

    [Then(@"el usuario es redireccionado a ""(.*)""")]
    public void ThenElUsuarioEsRedireccionadoA(string p0)
    {
        throw new PendingStepException();
    }


}
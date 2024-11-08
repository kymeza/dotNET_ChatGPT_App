
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace SecurityTests.SecurityBehaviorTest.Steps;

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
        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
    }


    [AfterScenario]
    public void TearDown()
    {
       //_driver.Quit();
    }

    // Scenario: Login Exitoso por parte del usuario

    [Given(@"el usuario navega a ""(.*)""")]
    public void DadoQueElUsuarioNavegaA(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    [When(@"el usario ingresa ""(.*)"" dentro del campo de usuario")]
    public void CuandoElUsuarioIngresaUsuarioEnElCampoDeUsuario(string userName)
    {
        throw new PendingStepException();
    }


    [When(@"el usuario ingresa ""(.*)"" dentro del campo de contraseña")]
    public void CuandoElUsuarioIngresaContrasenaEnElCampoContrasena(string password)
    {
        throw new PendingStepException();
    }

    [When(@"el usuario presiona Login")]
    public void CuandoElUsuarioPresionaLogin()
    {
        throw new PendingStepException();
    }

    [Then(@"el usuario debería ver la ruta ""([^""]*)"" en la app")]
    public void CuandoElUsuarioDeberiaVerLaRutaEnLaApp(string p0)
    {
        throw new PendingStepException();
    }

}
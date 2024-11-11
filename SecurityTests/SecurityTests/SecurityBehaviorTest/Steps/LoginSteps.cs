
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace SecurityTests.SecurityBehaviorTest.Steps;

[Binding]
public class LoginSteps
{

    private IWebDriver _driver;
    private WebDriverWait _driverWait;
    


    [BeforeScenario]
    public void SetUp()
    {
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArgument("--ignore-certificate-errors");


        _driver = new ChromeDriver(chromeOptions);

        _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(10);

        _driverWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));


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
        var campo = _driver.FindElement(By.Id("username"));
        campo.SendKeys(userName);
    }


    [When(@"el usuario ingresa ""(.*)"" dentro del campo de contraseña")]
    public void CuandoElUsuarioIngresaContrasenaEnElCampoContrasena(string password)
    {
        var campo = _driver.FindElement(By.XPath("//body/app-root/app-login/div/form/input[2]"));
        campo.SendKeys(password);
    }

    [When(@"el usuario presiona Login")]
    public void CuandoElUsuarioPresionaLogin()
    {
        var boton = _driver.FindElement(By.XPath("//body/app-root/app-login/div/form/button"));
        boton.Click();
    }

    [Then(@"el usuario debería ver la ruta ""([^""]*)"" en la app")]
    public void CuandoElUsuarioDeberiaVerLaRutaEnLaApp(string ruta)
    {
        // Retrieve console logs

        _driverWait.Until(driver => driver.Url.Contains("/chat"));

        var urlActual = _driver.Url;
        var rutaEsperada = urlActual.Contains("/chat");
        Assert.That(rutaEsperada);

    
    }
}
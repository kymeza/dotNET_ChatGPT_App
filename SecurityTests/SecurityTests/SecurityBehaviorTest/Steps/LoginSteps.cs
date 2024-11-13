
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

    private List<bool> resultadosAtaqueDiccionarios;


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


    // ******* ESCENARIO DE ATAQUE AUTOMATIZADO HACKING ETICO *******

    [Given(@"el atacante navega a ""(.*)""")]
    public void GivenElAtacanteNavegaA(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    [When(@"el atacante ingresa un diccionario de usuarios y un diccionario de contraseñas en los campos respectivos y presiona login")]
    public async Task WhenElAtacanteIngresaDentroDelCampoDeUsuario()
    {

        var campoUsername = _driver.FindElement(By.Id("username"));
        var campoPassword = _driver.FindElement(By.XPath("//body/app-root/app-login/div/form/input[2]"));
        var botonLogin = _driver.FindElement(By.XPath("//body/app-root/app-login/div/form/button"));

        var usernamesDictionary = await File.ReadAllLinesAsync("1kCommonUsernames.txt");
        var passwordsDictionary = await File.ReadAllLinesAsync("100kCommonPasswords.txt");

        resultadosAtaqueDiccionarios = new List<bool>();


        foreach (var user in usernamesDictionary)
        {
            campoUsername.Clear();
            campoUsername.SendKeys(user);
            foreach (var password in passwordsDictionary)
            {
                campoPassword.Clear();
                campoPassword.SendKeys(password);
                botonLogin.Click();

                var errorMensaje = _driver.FindElement(By.XPath("//body/app-root/app-login/div/form/div"));
                if (errorMensaje.Text != "Invalid username or password")
                {
                    resultadosAtaqueDiccionarios.Add(true);
                }

            }
        }
    }

    [Then("el atacante debería ver un mensaje de error: {string}")]
    public void ThenElAtacanteDeberiaVerUnMensajeDeError(string p0)
    {
        var conteoResultadosAtaqueDiccionario = resultadosAtaqueDiccionarios.Count;

        if (conteoResultadosAtaqueDiccionario > 0)
        {
            Assert.Fail("El atacante logra romper el Login de la Pagina");
        }

        Assert.That(true,"El atacante no logra romper el Login de la pagina");

    }

    [Then("el atacante no debería ver una traza de excepcion")]
    public void ThenElAtacanteNoDeberiaVerUnaTrazaDeExcepcion()
    {
        throw new PendingStepException();
    }




}
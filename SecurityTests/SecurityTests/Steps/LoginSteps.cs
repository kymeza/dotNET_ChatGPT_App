using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Reqnroll;

namespace SecurityTests.Steps;

[Binding]
public class LoginSteps
{

    private IWebDriver _driver;
    private WebDriverWait _driverWait;

    struct intentoContrasena
    {
        public string username;
        public string password;
        public bool exitoso;
    }

    private List<intentoContrasena> intentosExitosos;


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

        _driverWait.Until(d => d.Url.Contains("/chat"));

        var urlActual = _driver.Url;

        var estoyEnLaUrlDeseada = urlActual.Contains("/chat");

        Assert.That(estoyEnLaUrlDeseada);

    }

    // ***** ESCENARIO DE UN ATACANTE USANDO UN ATAQUE DE DICCIONARIOS EN LA APP *****


    [Given(@"Un atacante entra a la url ""(.*)""")]
    public void GivenUnAtacanteEntraALaUrl(string url)
    {
        _driver.Navigate().GoToUrl(url);
    }

    [When("Un atacante ataca los inputs de username y password con diccionarios y presiona login en cada intento")]
    public async Task WhenElAtacanteIngresaEnElCampoDeUsername()
    {
        var usernameInput = _driver.FindElement(By.Id("username"));
        var passwordInput = _driver.FindElement(By.XPath("/html/body/app-root/app-login/div/form/input[2]"));
        var botonLogin = _driver.FindElement(By.XPath("/html/body/app-root/app-login/div/form/button"));

        var usernamesDictionary = await File.ReadAllLinesAsync("Resources/1kCommonUsernames.txt");
        var passwordsDictionary = await File.ReadAllLinesAsync("Resources/100kCommonPasswords.txt");

        intentosExitosos = new List<intentoContrasena>();

        foreach (var user in usernamesDictionary)
        {
            usernameInput.Clear();
            usernameInput.SendKeys(user);
            foreach (var password in passwordsDictionary)
            {
                passwordInput.Clear();
                passwordInput.SendKeys(password);
                botonLogin.Click();

                var messageElement = _driver.FindElement(By.XPath("/html/body/app-root/app-login/div/form/div"));

                if (messageElement.Text != "Incorrect username or password")
                {
                    intentoContrasena intento = new intentoContrasena()
                    {
                        username = user,
                        password = password,
                        exitoso = true
                    };
                    intentosExitosos.Add(intento);
                }
            }
        }
    }

    [Then(@"el atacante no debería tener intentos exitosos")]
    public void ThenElAtacanteNoDeberiaTenerIntentosExitosos()
    {
        // Print los intentos exitosos en consola o en otro lugar
        Assert.That(intentosExitosos.Count == 0);
    }

    [Then("el atacante NO DEBERÍA ver una excepcion en consola.")]
    public void ThenElAtacanteNODEBERIAVerUnaExcepcionEnConsola_()
    {
        throw new PendingStepException();
    }



}
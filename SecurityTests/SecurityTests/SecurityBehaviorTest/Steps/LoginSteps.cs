
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SecurityTests.SecurityBehaviorTest.Steps;

[Binding]
public class LoginSteps
{
    [BeforeScenario]
    public void SetUp()
    {
        //Configurar el comienzo del escenario de testeo
    }


    [AfterScenario]
    public void TearDown()
    {
        // Configurar la salida del testeo de algun escenario
    }

    // Scenario: Login Exitoso por parte del usuario

    [Given(@"el usuario navega a ""(.*)""")]
    public void DadoQueElUsuarioNavegaA(string url)
    {

    }

    [When(@"el usario ingresa ""(.*)"" dentro del campo de usuario")]
    public void CuandoElUsuarioIngresaUsuarioEnElCampoDeUsuario(string userName)
    {

    }


    [When(@"el usuario ingresa ""(.*)"" dentro del campo de contraseña")]
    public void CuandoElUsuarioIngresaContrasenaEnElCampoContrasena(string password)
    {

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
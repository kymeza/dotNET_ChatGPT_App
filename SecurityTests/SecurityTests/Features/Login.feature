Feature: User Login

	Scenario: Un usuario logea de manera exitosa
		Given Un usuario entra a la url "https://localhost:7288/login"
		When el usuario ingresa "kmeza" en el campo de username
		And el usuario ingresa "asdf.1234" en el campo de password
		And el usuario presiona el boton login
		Then el usuario es redireccionado a "/chat"

	Scenario: Un usuario logea de manera erronea por username incorrecto
		Given Un usuario entra a la url "https://localhost:7288/login"
		When  el usuario ingresa "notValid" en el campo de username
		And  el usuario ingresa "notValid" en el campo de password
		And  el usuario presiona el boton login
		Then  el usuario DEBERÍA ver el mensaje "Incorrect username or password"

	Scenario: Un usuario logea de manera erronea por pasword incorrecta
		Given Un usuario entra a la url "https://localhost:7288/login"
		When  el usuario ingresa "kmeza" en el campo de username
		And  el usuario ingresa "1234.asdf.1234" en el campo de password
		And  el usuario presiona el boton login
		Then  el usuario DEBERÍA ver el mensaje "Incorrect username or password"

	Scenario: Un atacante intenta inyectar sql en el login
		Given Un atacante entra a la url "https://localhost:7288/login"
		When  el atacante ingresa " ' OR '1'='1 " en el campo de username
		And  el atacante ingresa "1234.asdf.1234" en el campo de password
		And  el atacante presiona el boton login
		Then  el atacante DEBERÍA ver el mensaje "Incorrect username or password"
		Then  el atacante NO DEBERÍA ver una excepcion en consola.

	Scenario: Un atacante intenta usar un diccionario de usuarios y contraseñas
		Given Un atacante entra a la url "https://localhost:7288/login"
		When  el atacante ingresa las entradas del diccionario en el campo de username
		And  el atacante ingresa "1234.asdf.1234" en el campo de password
		And  el atacante presiona el boton login
		Then  el atacante DEBERÍA ver el mensaje "Incorrect username or password"
		Then  el atacante NO DEBERÍA ver una excepcion en consola.


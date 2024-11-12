Feature: User Login

	Scenario: Un usuario logea de manera exitosa
		Given (Dado que) un usuario entra a la url "https://localhost:7288/login"
		When (Cuando) el usuario ingresa "kmeza" en el campo de username
		And (Y) el usuario ingresa "asdf.1234" en el campo de password
		And (Y) el usuario presiona el boton login
		Then (Entonces) el usuario es redireccionado a "/chat"

	Scenario: Un usuario logea de manera erronea por username incorrecto
		Given (Dado que) un usuario entra a la url "https://localhost:7288/login"
		When (Cuando) el usuario ingresa "notValid" en el campo de username
		And (Y) el usuario ingresa "notValid" en el campo de password
		And (Y) el usuario presiona el boton login
		Then (Entonces) el usuario DEBERÍA ver el mensaje "Incorrect username or password"

	Scenario: Un usuario logea de manera erronea por pasword incorrecta
		Given (Dado que) un usuario entra a la url "https://localhost:7288/login"
		When (Cuando) el usuario ingresa "kmeza" en el campo de username
		And (Y) el usuario ingresa "1234.asdf.1234" en el campo de password
		And (Y) el usuario presiona el boton login
		Then (Entonces) el usuario DEBERÍA ver el mensaje "Incorrect username or password"

	Scenario: Un atacante intenta inyectar sql en el login
		Given (Dado que) un atacante entra a la url "https://localhost:7288/login"
		When (Cuando) el atacante ingresa " ' OR '1'='1 " en el campo de username
		And (Y) el atacante ingresa "1234.asdf.1234" en el campo de password
		And (Y) el atacante presiona el boton login
		Then (Entonces) el atacante DEBERÍA ver el mensaje "Incorrect username or password"
		Then (Entonces) el atacante NO DEBERÍA ver una excepcion en consola.

	Scenario: Un atacante intenta usar un diccionario de inyeccion sql en el login
		Given (Dado que) un atacante entra a la url "https://localhost:7288/login"
		When (Cuando) el atacante ingresa las entradas del diccionario en el campo de username
		And (Y) el atacante ingresa "1234.asdf.1234" en el campo de password
		And (Y) el atacante presiona el boton login
		Then (Entonces) el atacante DEBERÍA ver el mensaje "Incorrect username or password"
		Then (Entonces) el atacante NO DEBERÍA ver una excepcion en consola.


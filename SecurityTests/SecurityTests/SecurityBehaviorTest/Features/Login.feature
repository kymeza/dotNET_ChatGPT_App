Feature: Login de un Usuario
	
	Scenario: Login Exitoso por parte del usuario
		Given el usuario navega a "https://localhost:7288/login"
		When el usario ingresa "kmeza" dentro del campo de usuario
		And el usuario ingresa "asdf.1234" dentro del campo de contraseña
		And el usuario presiona Login
		Then el usuario debería ver la ruta "/chat" en la app

	Scenario: Login erroneo por parte del usuario por usuario incorrecto
		Given el usuario navega a "https://localhost:7288/login"
		When el usario ingresa "asdfasdf" dentro del campo de usuario
		And el usuario ingresa "asdf.1234" dentro del campo de contraseña
		And el usuario presiona Login
		Then el usuario debería ver un mensaje de error: "Invalid username or password"
		And el usuraio no debería ver una traza de excepcion

	Scenario: Login erroneo por parte del usuario por contraseña incorrecta
		Given el usuario navega a "https://localhost:7288/login"
		When el usario ingresa "kmeza" dentro del campo de usuario
		And el usuario ingresa "1234.asdf.1234" dentro del campo de contraseña
		And el usuario presiona Login
		Then el usuario debería ver un mensaje de error: "Invalid username or password"
		And el usuraio no debería ver una traza de excepcion

	Scenario: Login erroneo por parte del usuario de manera repetida (3 veces)
		Given el usuario navega a "https://localhost:7288/login"
		When el usario ingresa "kmeza" dentro del campo de usuario
		And el usuario ingresa "1234.asdf.1234" dentro del campo de contraseña
		And el usuario presiona Login
		Then el usuario debería ver un mensaje de error: "Su cuenta ha sido bloqueada temporalmente debido a razones de seguridad"
		And el usuraio no debería ver una traza de excepcion

	Scenario: Un atacante intena logear usando SQL Injection con un diccionario
		Given el atacante navega a "https://localhost:7288/login"
		When el atacante ingresa "" dentro del campo de usuario
		And el atacante ingresa sus payload de inyeccion dentro del campo de contraseña
		And el atacante presiona Login
		Then el atacante debería ver un mensaje de error: "Invalid username or password"
		And el atacante no debería ver una traza de excepcion
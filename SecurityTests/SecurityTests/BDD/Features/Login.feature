Feature: User Login

    Scenario: Successful login
        Given the user navigates to "http://example.com/login"
        When the user enters "testuser" into the "username" field
        And the user enters "password123" into the "password" field
        And the user clicks the "Login" button
        Then the user should see "Welcome, testuser" on the page
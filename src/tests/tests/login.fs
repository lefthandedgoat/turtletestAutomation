module login

open canopy
open canopyExtensions
open common
open runner
open page_login

let all () =
  context "login"

  let mutable name, email = "",""

  once (fun _ ->
    page_register.generateUniqueUser' &name &email
    page_register.register name email)

  before (fun _ -> goto page_login.uri)

  "Register takes you to register page" &&& fun _ ->
    click _register
    on page_register.uri

  "Email is prepopulated on failed login, but password is not" &&& fun _ ->
    _email << "a@b.com"
    _password << "654321"
    click _login

    displayed _invalid
    _email == "a@b.com"
    _password == ""

  "Email required" &&& fun _ ->
    _password << "654321"
    click _login
    displayed _invalid

  "Password required" &&& fun _ ->
    _email << email
    click _login
    displayed _invalid

  "Valid password required" &&& fun _ ->
    _email << email
    _password << "654321"
    click _login
    displayed _invalid

  "Valid credentials takes you to home page" &&& fun _ ->
    _email << email
    _password << "test1234"
    click _login

    on (page_home.uri name)

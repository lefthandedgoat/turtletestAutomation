module register

open canopy
open canopyExtensions
open common
open runner
open page_register

let all () =
  context "register"

  before (fun _ -> goto page_register.uri)

  "Name required" &&& fun _ ->
    click _submit
    displayed _nameRequired

  "Name invalid" &&& fun _ ->
    name 65 Invalid
    name 66 Invalid

  "Name valid" &&& fun _ ->
    name 1 Valid
    name 2 Valid
    name 63 Valid
    name 64 Valid

  "Email required" &&& fun _ ->
    click _submit
    displayed _emailNotValid

  "Email invalid" &&& fun _ ->
    _email << "junk"
    click _submit

    displayed _emailNotValid

  "Email valid" &&& fun _ ->
    click _submit
    displayed _emailNotValid

    _email << "junk@null.dev"
    click _submit
    notDisplayed _emailNotValid

  "Password required" &&& fun _ ->
    click _submit
    displayed _password6to10

  "Password invalid" &&& fun _ ->
    password 1 Invalid
    password 2 Invalid
    password 3 Invalid
    password 4 Invalid
    password 5 Invalid
    password 101 Invalid

  "Password valid" &&& fun _ ->
    password 6 Valid
    password 7 Valid
    password 99 Valid
    password 100 Valid

  "Password mismatch" &&& fun _ ->
    _password << "123456"
    _repeat << "654321"
    click _submit
    displayed _passwordsMatch

  "Can register new unique user" &&& fun _ ->
    let username, email = generateUniqueUser ()

    _name << username
    _email << email
    _password << "test1234"
    _repeat << "test1234"
    click _submit

    on (page_home.uri username)

  "Duplicate registrations are not allowed" &&& fun _ ->
    let username, email = generateUniqueUser ()

    register username email
    tryRegister username email

    displayed _nameTaken
    displayed _emailTaken

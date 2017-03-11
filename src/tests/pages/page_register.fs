module page_register

open canopy
open canopyExtensions
open common

let uri = common.baseuri + "register"

//selectors
let _name = placeholder "Name"
let _email = placeholder "Email"
let _password = placeholder "Password"
let _repeat = placeholder "Repeat Password"
let _submit = value "Submit"

//validation messages
let _nameRequired = text "Name is required"
let _name64 =  text "Name must be 64 characters or less"
let _nameTaken =  text "Name is already taken"
let _emailNotValid =  text "Email not valid"
let _emailTaken =  text "Email is already taken"
let _password6to10 = text "Password must be between 6 and 100 characters"
let _passwordsMatch = text "Passwords must match"

//helpers
let generateUniqueUser () =
  let letters = genChars 10
  let email = sprintf "test_%s@null.dev" letters
  let name = sprintf "test_%s" letters
  name, email

//this is kinda hacky but it cleans up code well enough
let generateUniqueUser' (name : byref<string>) (email : byref<string>) =
  let name', email' = generateUniqueUser ()
  name <- name'
  email <- email'

let name length validation =
  goto uri

  match validation with
  | Valid -> //force failure so we make sure that he error goes away when valid
    click _submit
    displayed _nameRequired
    _name << genChars length
    click _submit
    notDisplayed _name64
  | Invalid ->
    _name << genChars length
    click _submit
    displayed _name64

let password length validation =
  goto uri

  match validation with
  | Valid -> //force failure so we make sure that he error goes away when valid
    _password << genChars 1
    click _submit
    displayed _password6to10
    let password = genChars length
    _password << password
    _repeat << password
    click _submit
    notDisplayed _password6to10
  | Invalid ->
    _password << genChars length
    click _submit
    displayed _password6to10

let tryRegister username email =
  goto uri

  _name << username
  _email << email
  _password << "test1234"
  _repeat << "test1234"

  click _submit

let register username email =
  tryRegister username email
  on (page_home.uri username)

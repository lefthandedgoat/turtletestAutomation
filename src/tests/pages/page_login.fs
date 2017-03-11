module page_login

open canopy
open canopyExtensions

let uri = common.baseuri + "login"

//selectors
let _email = placeholder "Email"
let _password = placeholder "Password"
let _login = value "Login"
let _register = text "Register"

//validations
let _invalid = "Invalid email or password"

//helpers
let logout () =
  goto (common.baseuri + "logout")
  on uri

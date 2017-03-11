module applications

open canopy
open canopyExtensions
open common
open runner
open page_applications

let all () =
  context "applications"

  let mutable name, email = "",""

  once (fun _ ->
    page_register.generateUniqueUser' &name &email
    page_register.register name email)

  before (fun _ -> goto (page_applications.uri name))

  "When you go there directly, and there is an application, it takes you to the create page" &&& fun _ ->
    on (page_applicationCreate.uri name)

  "After creating an application, it is in the grid" &&& fun _ ->
    page_applicationCreate.createRandom name Public
    goto (page_applications.uri name)
    count _rows 1

  "Grid has the right _columns" &&& fun _ ->
    count _columns 4
    _columns *= "Name"
    _columns *= "Owners"
    _columns *= "Developers"
    _columns *= "Private"

  "Upon clicking the application, it takes you to the details" &&& fun _ ->
    click _rows
    on (page_application.uriRoot name)

  "Clicking the create button takes you the create page" &&& fun _ ->
    click _create
    on (page_applicationCreate.uri name)

  "When logged out, there is no Create button" &&& fun _ ->
    page_login.logout()
    goto (page_applications.uri name)
    notDisplayed _create

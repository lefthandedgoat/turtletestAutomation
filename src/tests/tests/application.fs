module application

open canopy
open canopyExtensions
open common
open runner
open page_application

let all () =
  context "application"

  let mutable name, email = "",""
  let mutable appUrl = ""
  let mutable suiteUrl = ""

  once (fun _ ->
    page_register.generateUniqueUser' &name &email
    page_register.register name email
    page_applicationCreate.createRandom name Public
    appUrl <- currentUrl()
    page_suiteCreate.createRandom name Recent
    suiteUrl <- currentUrl())

  before (fun _ -> goto appUrl)

  "Data is set properly" &&& fun _ ->
    _address == "http://www.turtletest.com"
    _documentation == "http://www.turtletest.com"
    _owners == "todo"
    _developers == "Bob"
    _notes == "Some notes"
    _private == "False"

  "Edit button takes you to edit page" &&& fun _ ->
    click _edit
    on (page_applicationEdit.uriRoot name)

  "Create button takes you to create page" &&& fun _ ->
    click _create
    on (page_applicationCreate.uri name)

  "Suite grid has the right columns" &&& fun _ ->
    count _columns 3
    _columns *= "Name"
    _columns *= "Version"
    _columns *= "Owners"

  "Suite grid takes you to the suite page" &&& fun _ ->
    click _rows
    on (page_suite.uriRoot name)

  "Suite create takes you to suite create page" &&& fun _ ->
    click _secondCreate
    on (page_suiteCreate.uri name)

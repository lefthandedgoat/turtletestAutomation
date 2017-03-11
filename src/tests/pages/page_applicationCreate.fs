module page_applicationCreate

open canopy
open canopyExtensions
open common

let uri name = sprintf "%s%s/application/create" common.baseuri name

//selectors
let _name = placeholder "Name"
let _address = placeholder "Address"
let _documentation = placeholder "Documentation"
let _owners = placeholder "Owners"
let _developers = placeholder "Developers"
let _notes = placeholder "Notes"
let _private = name "Private"
let _save = value "Save"

let create name appName address documentation owners developers notes private' =
  goto (uri name)
  _name << appName
  _address << address
  _documentation << documentation
  _owners << owners
  _developers << developers
  _notes << notes
  _private << private'
  click _save
  on (page_application.uriRoot name)

let createRandom name privacy =
  create
    name
    (sprintf "%s %s" (genChars 7) (genChars 8))
    "http://www.turtletest.com"
    "http://www.turtletest.com"
    "todo"
    "Bob"
    "Some notes"
    (match privacy with Public -> "No" | Private -> "Yes")

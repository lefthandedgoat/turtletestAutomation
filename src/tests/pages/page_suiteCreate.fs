module page_suiteCreate

open canopy
open canopyExtensions
open common

let uri name = sprintf "%s%s/suite/create" common.baseuri name

let _application = name "Application"
let _applications = options "Application"
let _name = placeholder "Name"
let _version = placeholder "Version"
let _owners = placeholder "Owners"
let _notes = placeholder "Notes"
let _save = value "Save"

let create name option suiteName version owners notes =
  goto (uri name)

  let appOptionValue =
    match option with
    | First -> firstOption _applications
    | Recent -> recentOption _applications

  _application << appOptionValue
  _name << suiteName
  _version << version
  _owners << owners
  _notes << notes
  click _save

let createRandom name option =
  create
    name
    option
    (sprintf "%s %s" (genChars 7) (genChars 8))
    "1.0"
    "Sally"
    "Notes about the suite"

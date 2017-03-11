module page_testcaseCreate

open canopy
open canopyExtensions
open common

let uri name = sprintf "%s%s/testcase/create" common.baseuri name

let _application = name "Application"
let _applications = options "Application"
let _suite = name "Suite"
let _suites = options "Suite"
let _name = placeholder "Name"
let _version = placeholder "Version"
let _owners = placeholder "Owners"
let _notes = placeholder "Notes"
let _requirements = placeholder "Requirements"
let _steps = placeholder "Steps"
let _expected = placeholder "Expected"
let _history = placeholder "History"
let _attachments = placeholder "Attachments"
let _save = value "Save"

let create name option testcaseName version owners notes requirements steps expected history attachments =
  goto (uri name)

  let appOptionValue =
    match option with
    | First -> firstOption _applications
    | Recent -> recentOption _applications

  let suiteOptionValue =
    match option with
    | First -> firstOption _suites
    | Recent -> recentOption _suites

  _application << appOptionValue
  _suite << suiteOptionValue
  _name << testcaseName
  _version << version
  _owners << owners
  _notes << notes
  _requirements << requirements
  _steps << steps
  _expected << expected
  _history << history
  _attachments << attachments
  click _save

let createRandom name option =
  create
    name
    option
    (sprintf "%s %s" (genChars 7) (genChars 8))
    "1.0"
    "Sally"
    "Notes about the suite"
    "Some requirements"
    "Various steps"
    "The expectations"
    "Historical information"
    "attachments"

module page_testrunCreate

open canopy
open canopyExtensions
open common

let uri name = sprintf "%s%s/testrun/create" common.baseuri name

let _application = name "Application"
let _applications = options "Application"
let _description = placeholder "Description"
let _testCases = placeholder "Test Cases"
let _save = value "Save"

let create name option description testCases =
  goto (uri name)

  let appOptionValue =
    match option with
    | First -> firstOption _applications
    | Recent -> recentOption _applications

  _application << appOptionValue
  _description << description
  _testCases << testCases
  click _save

let createRandom name option =
  create
    name
    option
    (sprintf "%s %s" (genChars 7) (genChars 8))
    "This, that, something else"

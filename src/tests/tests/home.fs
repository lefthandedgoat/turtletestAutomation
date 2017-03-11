module home

open canopy
open canopyExtensions
open common
open runner
open page_home

let all () =
  context "home"

  let mutable name, email = "",""

  once (fun _ ->
    page_register.generateUniqueUser' &name &email
    page_register.register name email)

  before (fun _ -> goto (page_home.uri name))

  "After new registration, it shows that the person is logged in" &&& fun _ ->
    displayed (_hiName name)

  "When the user has a fresh account, applications link leads to create page" &&& fun _ ->
    click _applications_count
    on (page_applicationCreate.uri name)

  "When the user has a fresh account, suites link leads to create page" &&& fun _ ->
    click _suites_count
    on (page_suiteCreate.uri name)

  "When the user has a fresh account, test cases link leads to create page" &&& fun _ ->
    click _testCases_count
    on (page_testcaseCreate.uri name)

  "When the user has a fresh account, test runs link leads to create page" &&& fun _ ->
    click _testRuns_count
    on (page_testrunCreate.uri name)

  "When the user has a fresh account, counts are all 0" &&& fun _ ->
    _applications_count == "0"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"

  "After adding an public application, count increases to 1" &&& fun _ ->
    page_applicationCreate.createRandom name Public

    _applications_count == "1"
    _suites_count == "0"
    _testCases_count == "0"
    _testRuns_count == "0"

  "After adding suite, count increases to 1" &&& fun _ ->
    page_suiteCreate.createRandom name First

    _applications_count == "1"
    _suites_count == "1"
    _testCases_count == "0"
    _testRuns_count == "0"

  "After adding test case, count increases to 1" &&& fun _ ->
    page_testcaseCreate.createRandom name First

    _applications_count == "1"
    _suites_count == "1"
    _testCases_count == "1"
    _testRuns_count == "0"

  "After adding test run, count increases to 1" &&& fun _ ->
    page_testrunCreate.createRandom name First

    _applications_count == "1"
    _suites_count == "1"
    _testCases_count == "1"
    _testRuns_count == "1"

  "After adding an private application, count increases to 2" &&& fun _ ->
    page_applicationCreate.createRandom name Private

    _applications_count == "2"
    _suites_count == "1"
    _testCases_count == "1"
    _testRuns_count == "1"

  "After adding suite to private app, count increases to 2" &&& fun _ ->
    page_suiteCreate.createRandom name Recent

    _applications_count == "2"
    _suites_count == "2"
    _testCases_count == "1"
    _testRuns_count == "1"

  "After adding test case to private app, count increases to 2" &&& fun _ ->
    page_testcaseCreate.createRandom name Recent

    _applications_count == "2"
    _suites_count == "2"
    _testCases_count == "2"
    _testRuns_count == "1"

  "After adding test run to private app, count increases to 2" &&& fun _ ->
    page_testrunCreate.createRandom name Recent

    _applications_count == "2"
    _suites_count == "2"
    _testCases_count == "2"
    _testRuns_count == "2"

  "Tile Counts are correct" &&& fun _ ->
    _applications_tileCount == "2"
    _suites_tileCount == "2"
    _testCases_tileCount == "2"
    _testRuns_tileCount == "2"

  "When the user has an application, applications link leads to list page" &&& fun _ ->
    click _applications_count
    on (page_applications.uri name)

  "When the user has a suite, suites link leads to list page" &&& fun _ ->
    click _suites_count
    on (page_suites.uri name)

  "When the user has a test case, test cases link leads to list page" &&& fun _ ->
    click _testCases_count
    on (page_testcases.uri name)

  "When the user has a test run, test runs link leads to list page" &&& fun _ ->
    click _testRuns_count
    on (page_testruns.uri name)

  "Clicking the hi name link in the bottom left logs the user out" &&& fun _ ->
    click (_hiName name)
    on page_login.uri

  "After logging out, the number of applications and suites should be 1" &&& fun _ ->
    _applications_count == "1"
    _suites_count == "1"
    _testCases_count == "1"
    _testRuns_count == "1"

  "After logging out, Tile Counts are correct" &&& fun _ ->
    _applications_tileCount == "1"
    _suites_tileCount == "1"
    _testCases_tileCount == "1"
    _testRuns_tileCount == "1"

  "In logged out state, clicking the login button takes you to the login screen" &&& fun _ ->
    click _login
    on page_login.uri

//todo add some tests for the test runs grid once its implimented

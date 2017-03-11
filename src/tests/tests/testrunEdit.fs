module testrunEdit

open canopy
open canopyExtensions
open common
open runner
open page_testrunEdit

let all () =
  context "test run edit"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

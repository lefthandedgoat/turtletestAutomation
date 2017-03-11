module testrun

open canopy
open canopyExtensions
open common
open runner
open page_testrun

let all () =
  context "test run"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

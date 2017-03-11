module testcase

open canopy
open canopyExtensions
open common
open runner
open page_testcase

let all () =
  context "test case"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

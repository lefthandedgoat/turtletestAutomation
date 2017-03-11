module testcaseEdit

open canopy
open canopyExtensions
open common
open runner
open page_testcaseEdit

let all () =
  context "test case edit"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

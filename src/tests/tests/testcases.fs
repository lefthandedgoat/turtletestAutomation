module testcases

open canopy
open canopyExtensions
open common
open runner

let all () =
  context "test cases"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

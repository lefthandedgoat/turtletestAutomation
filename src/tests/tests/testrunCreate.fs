module testrunCreate

open canopy
open canopyExtensions
open common
open runner

let all () =
  context "test run create"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

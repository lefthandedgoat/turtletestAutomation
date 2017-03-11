module suiteCreate

open canopy
open canopyExtensions
open common
open runner
open page_suiteCreate

let all () =
  context "suite create"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

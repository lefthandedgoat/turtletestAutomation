module suiteEdit

open canopy
open canopyExtensions
open common
open runner
open page_suiteEdit

let all () =
  context "suite create"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

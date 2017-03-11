module suites'

open canopy
open canopyExtensions
open common
open runner
open page_suites

let all () =
  context "suites"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

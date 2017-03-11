module applicationCreate

open canopy
open canopyExtensions
open common
open runner
open page_applicationCreate

let all () =
  context "application create"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

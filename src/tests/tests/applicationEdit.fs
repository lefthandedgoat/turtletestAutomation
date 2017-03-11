module applicationEdit

open canopy
open canopyExtensions
open common
open runner
open page_applicationEdit

let all () =
  context "application edit"

  "test 1" &&& fun _ ->
    url "http://turtletest.com/"

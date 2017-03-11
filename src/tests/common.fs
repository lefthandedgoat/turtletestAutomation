module common

open canopy

let baseuri = "http://www.turtletest.com/"

type Validation = Valid | Invalid

type Privacy = Public | Private

type SelectOption = First | Recent

let random = System.Random()
let private letters = [ 'a' .. 'z' ]

let genChars length =
  [| 1 .. length |] |> Array.map (fun _ -> letters.[random.Next(25)]) |> System.String

//selectors
let _rows = css "tbody tr"
let _columns = css "th"

let _create = text "Create"
let _edit = text "Edit"
let _secondCreate = xpath "//a[contains(@href, '/suite/create')]"

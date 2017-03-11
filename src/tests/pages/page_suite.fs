module page_suite

open canopy
open canopyExtensions

let uriRoot name = sprintf "%s%s/suite" common.baseuri name
let uri name id = sprintf "%s%s/suite/%i" common.baseuri name id

module page_applicationEdit

open canopy
open canopyExtensions

let uriRoot name = sprintf "%s%s/application/edit" common.baseuri name
let uri name id = sprintf "%s%s/application/edit/%i" common.baseuri name id

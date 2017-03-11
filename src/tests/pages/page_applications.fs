module page_applications

open canopy
open canopyExtensions
open common

let uri name = sprintf "%s%s/applications" common.baseuri name

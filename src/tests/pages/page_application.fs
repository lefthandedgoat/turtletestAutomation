module page_application

open canopy
open canopyExtensions

let uriRoot name = sprintf "%s%s/application" common.baseuri name
let uri name id = sprintf "%s%s/application/%i" common.baseuri name id

let _address = data_qa_name "Address"
let _documentation = data_qa_name "Documentation"
let _owners = data_qa_name "Owners"
let _developers = data_qa_name "Developers"
let _notes = data_qa_name "Notes"
let _private = data_qa_name "Private"

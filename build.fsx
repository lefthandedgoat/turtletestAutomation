// --------------------------------------------------------------------------------------
// FAKE build script
// --------------------------------------------------------------------------------------

#r @"packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.Git
open Fake.AssemblyInfoFile
open Fake.ReleaseNotesHelper
open Fake.UserInputHelper
open System
open System.IO
#if MONO
#else
#load "packages/SourceLink.Fake/tools/Fake.fsx"
open SourceLink
#endif

// --------------------------------------------------------------------------------------
// START TODO: Provide project-specific details below
// --------------------------------------------------------------------------------------

// Information about the project are used
//  - for version and project name in generated AssemblyInfo file
//  - by the generated NuGet package
//  - to run tests and to publish documentation on GitHub gh-pages
//  - for documentation, you also need to edit info in "docs/tools/generate.fsx"

// The name of the project
// (used by attributes in AssemblyInfo, name of a NuGet package and directory in 'src')
let project = "Site"

// Short summary of the project
// (used as description in AssemblyInfo and as a short summary for NuGet package)
let summary = "turtletest.com"

// Longer description of the project
// (used as a description for NuGet package; line breaks are automatically cleaned up)
let description = "turtletest.com"

// List of author names (for NuGet package)
let authors = [ "Chris Holt" ]

// Tags for your project (for NuGet package)
let tags = "demo"

// File system information
let solutionFile  = "Site.sln"

// Git configuration (used for publishing documentation in gh-pages branch)
// The profile where the project is posted
let gitOwner = "lefthandedgoat"
let gitHome = "https://github.com/" + gitOwner

// The name of the project on GitHub
let gitName = "turtletest"

// The url for the raw files hosted
let gitRaw = environVarOrDefault "gitRaw" "https://raw.github.com/lefthandedgoat"

// --------------------------------------------------------------------------------------
// END TODO: The rest of the file includes standard build steps
// --------------------------------------------------------------------------------------

// Read additional information from the release notes document
let release = LoadReleaseNotes "RELEASE_NOTES.md"

let testsBuildDir = "./bin/tests/"
let siteBuildDir = "./bin/Site/"

// Helper active pattern for project types
let (|Fsproj|Csproj|Vbproj|) (projFileName:string) =
    match projFileName with
    | f when f.EndsWith("fsproj") -> Fsproj
    | f when f.EndsWith("csproj") -> Csproj
    | f when f.EndsWith("vbproj") -> Vbproj
    | _                           -> failwith (sprintf "Project file %s not supported. Unknown project type." projFileName)

// Generate assembly info files with the right version & up-to-date information
Target "AssemblyInfo" (fun _ ->
    let getAssemblyInfoAttributes projectName =
        [ Attribute.Title (projectName)
          Attribute.Product project
          Attribute.Description summary
          Attribute.Version release.AssemblyVersion
          Attribute.FileVersion release.AssemblyVersion ]

    let getProjectDetails projectPath =
        let projectName = System.IO.Path.GetFileNameWithoutExtension(projectPath)
        ( projectPath,
          projectName,
          System.IO.Path.GetDirectoryName(projectPath),
          (getAssemblyInfoAttributes projectName)
        )

    !! "src/**/*.??proj"
    |> Seq.map getProjectDetails
    |> Seq.iter (fun (projFileName, projectName, folderName, attributes) ->
        match projFileName with
        | Fsproj -> CreateFSharpAssemblyInfo (folderName @@ "AssemblyInfo.fs") attributes
        | Csproj -> CreateCSharpAssemblyInfo ((folderName @@ "Properties") @@ "AssemblyInfo.cs") attributes
        | Vbproj -> CreateVisualBasicAssemblyInfo ((folderName @@ "My Project") @@ "AssemblyInfo.vb") attributes
        )
)

// Copies binaries from default VS location to expected bin folder
// But keeps a subdirectory structure for each project in the
// src folder to support multiple project outputs
Target "CopyBinaries" (fun _ ->
    !! "src/**/*.??proj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) @@ "bin/Release", "bin" @@ (System.IO.Path.GetFileNameWithoutExtension f)))
    |>  Seq.iter (fun (fromDir, toDir) -> CopyDir toDir fromDir (fun _ -> true))
)

Target "CopyAssets" (fun _ ->
    !! "src/Site/*.fsproj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) @@ "css", "bin/Site/css"))
    |>  Seq.iter (fun (fromDir, toDir) -> printfn "from: %s, to: %s" fromDir toDir; CopyDir toDir fromDir (fun _ -> true))
    !! "src/Site/*.fsproj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) @@ "js", "bin/Site/js"))
    |>  Seq.iter (fun (fromDir, toDir) ->  printfn "from: %s, to: %s" fromDir toDir; CopyDir toDir fromDir (fun _ -> true))
    !! "src/Site/*.fsproj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) @@ "fonts", "bin/Site/fonts"))
    |>  Seq.iter (fun (fromDir, toDir) ->  printfn "from: %s, to: %s" fromDir toDir; CopyDir toDir fromDir (fun _ -> true))
    !! "src/Site/*.fsproj"
    |>  Seq.map (fun f -> ((System.IO.Path.GetDirectoryName f) @@ "images", "bin/Site/images"))
    |>  Seq.iter (fun (fromDir, toDir) ->  printfn "from: %s, to: %s" fromDir toDir; CopyDir toDir fromDir (fun _ -> true))
)

// --------------------------------------------------------------------------------------
// Clean build results

Target "Clean" (fun _ ->
    CleanDirs ["bin"; "temp"]
)

// --------------------------------------------------------------------------------------
// Build library & test project

Target "Build" (fun _ ->
    !! solutionFile
    |> MSBuildRelease "" "Rebuild"
    |> ignore
)

#if MONO
#else
// --------------------------------------------------------------------------------------
// SourceLink allows Source Indexing on the PDB generated by the compiler, this allows
// the ability to step through the source code of external libraries http://ctaggart.github.io/SourceLink/

Target "SourceLink" (fun _ ->
    let baseUrl = sprintf "%s/%s/{0}/%%var2%%" gitRaw project
    !! "src/**/*.??proj"
    |> Seq.iter (fun projFile ->
        let proj = VsProj.LoadRelease projFile
        SourceLink.Index proj.CompilesNotLinked proj.OutputFilePdb __SOURCE_DIRECTORY__ baseUrl
    )
)

#endif

// --------------------------------------------------------------------------------------
// Release Scripts

#load "paket-files/fsharp/FAKE/modules/Octokit/Octokit.fsx"
open Octokit

Target "Release" (fun _ ->
    let user =
        match getBuildParam "github-user" with
        | s when not (String.IsNullOrWhiteSpace s) -> s
        | _ -> getUserInput "Username: "
    let pw =
        match getBuildParam "github-pw" with
        | s when not (String.IsNullOrWhiteSpace s) -> s
        | _ -> getUserPassword "Password: "
    let remote =
        Git.CommandHelper.getGitResult "" "remote -v"
        |> Seq.filter (fun (s: string) -> s.EndsWith("(push)"))
        |> Seq.tryFind (fun (s: string) -> s.Contains(gitOwner + "/" + gitName))
        |> function None -> gitHome + "/" + gitName | Some (s: string) -> s.Split().[0]

    StageAll ""
    Git.Commit.Commit "" (sprintf "Bump version to %s" release.NugetVersion)
    Branches.pushBranch "" remote (Information.getBranchName "")

    Branches.tag "" release.NugetVersion
    Branches.pushTag "" remote release.NugetVersion

    // release on github
    createClient user pw
    |> createDraft gitOwner gitName release.NugetVersion (release.SemVer.PreRelease <> None) release.Notes
    // TODO: |> uploadFile "PATH_TO_FILE"
    |> releaseDraft
    |> Async.RunSynchronously
)


Target "RunTests" (fun _ ->
    let result =
        ExecProcess (fun info ->
            info.FileName <- (testsBuildDir @@ "tests.exe")
        ) (System.TimeSpan.FromMinutes 5.)

    if result <> 0 then failwith "Failed result from canopy unit tests"
)

Target "BuildPackage" DoNothing

// --------------------------------------------------------------------------------------
// Run all targets by default. Invoke 'build <Target>' to override

Target "All" DoNothing

Target "Default" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "CopyBinaries"
  ==> "CopyAssets"
  ==> "RunTests"
  ==> "All"

"All"
#if MONO
#else
  =?> ("SourceLink", Pdbstr.tryFind().IsSome )
#endif
  ==> "BuildPackage"

"BuildPackage"
  ==> "Release"

RunTargetOrDefault "Default"

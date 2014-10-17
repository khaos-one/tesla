<#
#>

#
# Functions
#
Function Make-Dir ($dir) {
    If (!(Test-Path $dir)) {
        $x = mkdir $dir
        Write-Host "Created $($x.FullName)"
    }
}

Function Move-File ($source, $target) {
    Move-Item -Force $source $target
    Write-Host "Moved $source -> $target"
}

#
# The script
# 

# Find the first solution file in the directory
If (!($solutionFile = Get-ChildItem -Force -Filter *.sln)) {
    Write-Host -ForegroundColor Red "[Error] No solution found."
    Exit
}

# Clear entire solution
If ($clearSolutionScript = Get-ChildItem -Force "ClearSolution.ps1") {
    Write-Host -ForegroundColor Green "[Build] Clearing the solution directory..."
    &$clearSolutionScript
}

# MSBuild invocation
Write-Host -ForegroundColor Green "[Build] Building solution $solutionFile..."
[void][System.Reflection.Assembly]::Load('Microsoft.Build.Utilities.v12.0, Version=12.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
$msbuild = [Microsoft.Build.Utilities.ToolLocationHelper]::GetPathToDotNetFrameworkFile("msbuild.exe", "VersionLatest")
&$msbuild /nologo /p:Configuration=Release /m $solutionFile.FullName

# Clean build
If ($cleanSolutionScript = Get-ChildItem -Force "CleanSolution.ps1") {
    Write-Host -ForegroundColor Green "[Build] Cleaning the build..."
    &$cleanSolutionScript
}

# Post-build assembly
Write-Host -ForegroundColor Green "[Build] Assembling..."

<#
Make-Dir .\Build
Make-Dir .\Build\Server
Make-Dir .\Build\Server\Logs
Move-File .\Server.Svc\bin\Release\* .\Build\Server
Move-File .\Server.Cli\bin\Release\Iridium.Server.Cli.exe .\Build\Server
Move-File .\Server.Cli\bin\Release\Iridium.Server.Cli.exe.config .\Build\Server

Make-Dir .\Build\Utilities
Move-File .\Utility.*\bin\Release\* .\Build\Utilities

Make-Dir .\Build\Admin
Move-File .\Admin\bin\Release\* .\Build\Admin

Make-Dir .\Build\Lib
Move-File .\Server\bin\Release\* .\Build\Lib
#>

# Done.
Write-Host -ForegroundColor Green "Done!"
Write-Host "Press any key to continue..."
$null = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

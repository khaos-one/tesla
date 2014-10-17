<#
.SYNOPSIS
	Recursively clears the entire solution of all build, cache, OS and such detritus.
.NOTES
	Author	: Egor 'khaos' Zelensky.
#>

$Script:Delete = @();
$Script:Exclude = @();

Function Add-DeleteItem($itemRegex) {
	$Script:Delete += "$itemRegex"
}
Set-Alias delete Add-DeleteItem

Function Add-ExcludeItem($itemRegex) {
	$Script:Exclude += "$itemRegex"
}
Set-Alias ignore Add-ExcludeItem

Function Exec-Deletion($itemRegex) {
	$items = Get-ChildItem -Force -Recurse
	ForEach ($i In $items) {
		$isExcluded = $false
		ForEach ($j In $Script:Exclude) {
			If ($i.FullName -imatch $j) {
				$isExcluded = $true
				#Write-Host -ForegroundColor Yellow "Skipped $($i.FullName)."
				Break
			}
		}
		If ($isExcluded) {
			Continue
		}
		ForEach ($k in $Script:Delete) {
			If ($i.FullName -imatch $k -and (Test-Path $i.FullName)) {
                Try {
				    Remove-Item -Force -Recurse $i.FullName
				    Write-Host -ForegroundColor Red "Deleted $($i.FullName)."
                }
                Catch {
                }

                Break
			}
		}
	}
}
Set-Alias exec Exec-Deletion

#
# Remove items.
#

# Visual Studio user-specific files
delete \.suo$
delete \.user$
delete \.sln\.docstates$

# Build results
delete \\bin$
delete \\obj$
delete \\debug$
delete \\release$
delete \\build$
delete \\x64$

# Preserve NuGet packages repository
ignore \\packages

# Preserve GIT
ignore \\\.git

# MSTest test results
delete \\TestResult
delete \\BuildLog\.

# Some temporary files
delete _i\.c$
delete _p\.c$
delete \.ilk$
delete \.meta$
delete \.obj$
delete \.pch$
delete \.pdb$
delete \.pgc$
delete \.pgd$
delete \.rsp$
delete \.sbr$
delete \.tlb$
delete \.tli$
delete \.tlh$
delete \.tmp$
delete \.tmp_proj$
delete \.log$
delete \.vspscc$
delete \.vssscc$
delete \.pidb$
delete \.scc$

# Visual C++ cache files
delete ^ipch$
delete \.aps$
delete \.ncb$
delete \.opensdf$
delete \.sdf$
delete \.cachefile$

# Visual Studio profiler
delete \.psess$
delete \.vsp$
delete \.vspx$

# Guidance Automation Toolkit garbage
delete \.gpstate$

# ReSharper stuff (.NET coding plug-in)
delete \\_ReSharper
delete \.ReSharper$

# TeamCity stuff (build add-in)
delete \\_TeamCity

# DotCover stuff (code coverage tool)
delete \.dotCover$

# NCrunch
delete \.ncrunch
delete crunch.*\.local\.xml$

# InstallShield stuff
delete \\Express

# Click-Once directory
delete \\publish

# DocProject (documentation generator add-in)
delete DocProject\\buildhelp$
delete DocProject\\Help\\.*\.HxT$
delete DocProject\\Help\\.*\.HxC$
delete DocProject\\Help\\.*\.hhc$
delete DocProject\\Help\\.*\.hhk$
delete DocProject\\Help\\.*\.hhp$
delete DocProject\\Help\\Html2$
delete DocProject\\Help\\html$

# Publish Web output
delete \.Publish\.xml$

# Windows Azure build output
delete csx
delete \.build\.csdef$

# Windows Store app package directory
delete \\AppPackages$

# Other stuff
delete \\sql$
delete \.Cache$
delete \\ClientBin$
delete StyleCop\.
delete \.dbmdl$
delete \.Publish\.xml$
delete \.pfx$
delete \.publishsettings$

# RIA/Silverlight projects
delete \\Generated_Code$

# Backup and report files from converting old projects
delete \\_UpgradeReport_Files$
delete \\Backup
delete UpgradeLog.*\.xml$
delete UpgradeLog.*\.htm$

# SQL Server files
delete \\App_Data\\.*\.mdf$
delete \\App_Data\\.*\.ldf$

# LightSwitch generated files
delete \\GeneratedArtifacts$
delete \\Pvt_Extensions$
delete ModelManifest\.xml$

# OS detritus
delete Thumbs\.db$
delete ehthumbs\.db$
delete Desktup\.ini$
delete RECYCLE\.BIN$
delete \.DS_Store$

# Execute deletion
exec

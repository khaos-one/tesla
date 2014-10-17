<#
.SYNOPSIS
	Recursively cleans build directories from debug and build detritus.
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

ignore \\\.git
ignore \\packages

delete \.pdb$
delete \.xml$
delete \.vshost

exec

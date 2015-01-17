$root = (split-path -parent $MyInvocation.MyCommand.Definition)
$version = [System.Reflection.Assembly]::LoadFile("$root\Validator\bin\Release\Validator.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)

Write-Host "Setting .nuspec version tag to $versionStr"

$content = (Get-Content $root\Validator.nuspec) 
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\MarkdownLog.compiled.nuspec

& $root\nuget.exe pack $root\MarkdownLog.compiled.nuspec
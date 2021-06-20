# Copyright (c) Stephen Hodgson. All rights reserved.
# Licensed under the MIT License. See LICENSE in the project root for license information.

$InputAuthor = Read-Host "Set Author name: (i.e. your name or username)"
$ProjectAuthor = "ProjectAuthor"

$InputName = Read-Host "Enter a name for your new project"
$ProjectName = "ProjectName"

$InputScope = Read-Host "Enter a scope for your new project (optional)"

if(-not [String]::IsNullOrWhiteSpace($InputScope)) {
  $InputScope = ".$InputScope"
}

$ProjectScope = ".ProjectScope"

Write-Host "Your new com$($InputScope.ToLower()).$($InputName.ToLower()) project is being created..."

$excludes = @('*Library*', '*Obj*','*InitializeTemplate*')

# Rename any directories before we crawl the folders
Remove-Item -Path ".\Readme.md"
Copy-Item -Path ".\$ProjectName\Packages\com$($ProjectScope.ToLower()).$($ProjectName.ToLower())\Readme.md" `
          -Destination ".\Readme.md"
Rename-Item -Path ".\$ProjectName\Packages\com$($ProjectScope.ToLower()).$($ProjectName.ToLower())" `
            -NewName "com$($InputScope.ToLower()).$($InputName.ToLower())"
Rename-Item -Path ".\$ProjectName" -NewName ".\$InputName"

Get-ChildItem -Path "*"-File -Recurse -Exclude $excludes | ForEach-Object -Process {
  $isValid = $true

  foreach ($exclude in $excludes) {
    if ((Split-Path -Path $_.FullName -Parent) -ilike $exclude) {
      $isValid = $false
      break
    }
  }

  if ($isValid) {
    Get-ChildItem -Path $_ -File | ForEach-Object -Process {
      $updated = $false;

      $fileContent = Get-Content $($_.FullName) -Raw

      # Rename all PascalCase instances
      if ($fileContent -cmatch $ProjectName) {
        $fileContent -creplace $ProjectName, $InputName | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      if ($fileContent -cmatch $ProjectScope) {
        $fileContent -creplace $ProjectScope, $InputScope | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      if ($fileContent -cmatch $ProjectAuthor) {
        $fileContent -creplace $ProjectAuthor, $InputAuthor | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      # Rename all lowercase instances
      if ($fileContent -cmatch $ProjectName.ToLower()) {
        $fileContent -creplace $ProjectName.ToLower(), $InputName.ToLower() | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      if ($fileContent -cmatch $ProjectScope.ToLower()) {
        $fileContent -creplace $ProjectScope.ToLower(), $InputScope.ToLower() | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      # Rename all UPPERCASE instances
      if ($fileContent -cmatch $ProjectName.ToUpper()) {
        $fileContent -creplace $ProjectName.ToUpper(), $InputName.ToUpper() | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      if ($fileContent -cmatch $ProjectScope.ToUpper()) {
        $fileContent -creplace $ProjectScope.ToUpper(), $InputScope.ToUpper() | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      # Update guids
      if ($fileContent -match "#INSERT_GUID_HERE#") {
        $fileContent -replace "#INSERT_GUID_HERE#", [guid]::NewGuid() | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      $fileContent = Get-Content $($_.FullName) -Raw

      # Update year
      if ($fileContent -match "#CURRENT_YEAR#") {
        $fileContent -replace "#CURRENT_YEAR#", (Get-Date).year | Set-Content $($_.FullName) -NoNewline
        $updated = $true
      }

      # Rename files
      if ($_.Name -match $ProjectName) {
        Rename-Item -LiteralPath $_.FullName -NewName ($_.Name -replace ($ProjectName, $InputName))
        $updated = $true
      }

      if ($updated) {
        Write-Host $_.Name
      }
    }
  }
}

Remove-Item -Path "InitializeTemplate.ps1"

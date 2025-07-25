param (
    [Parameter(Mandatory = $true, Position = 0)]
    [ValidateSet("install", "console", "html")]
    [string]$Command,
    [Parameter(Mandatory = $false, Position = 1)]
    [string]$Name1,
    [Parameter(Mandatory = $false, Position = 2)]
    [string]$Name2
)

function Install-LlmModel {
    Start-Ollama
    $model = Get-Content "src\model.txt" -Raw
    ollama pull $model
}

function Install-NpmPackages {
    npm install --no-fund
}

function Install-Ollama {
    $packageId = "Ollama.Ollama"
    $installed = Get-WinGetPackage -Id $packageId
    if ($installed) {
        Upgrade-WinGetPackage -Id $packageId
    } else {
        Install-WinGetPackage -Id $packageId
    }
}

function Install-WinGet {
    $winget = Get-Module -Name Microsoft.WinGet.Client -ErrorAction SilentlyContinue
    if (-not $winget) {
        Restart-AsAdmin

        $nuget = Get-PackageProvider -Name NuGet -ErrorAction SilentlyContinue
        if (-not $nuget) {
            Install-PackageProvider -Name NuGet -ErrorAction SilentlyContinue
        }

        Install-Module -Name Microsoft.WinGet.Client -ErrorAction SilentlyContinue
    }
}

function Invoke-Console([string] $Name1, [string] $Name2) {
    Publish-App
    Start-Ollama
    node out\console-app.js $Name1 $Name2
}

function Invoke-HtmlHost {
    Publish-App
    Start-Ollama
    node out\html-app.js
}

function Invoke-Install {
    Install-NpmPackages
    Install-WinGet
    Install-Ollama
    Install-LlmModel
}

function Publish-App {
    Remove-Item -Path "out" -Recurse -Force
    npx tsc
    Copy-Item -Path "src\model.txt" -Destination "out\model.txt" -Force
}

function Restart-AsAdmin {
    $currentUser = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent())
    if ( -not $currentUser.IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
        $scriptPath = $MyInvocation.MyCommand.Definition
        Start-Process powershell -ArgumentList "-NoProfile -ExecutionPolicy Bypass -File `"$scriptPath`"" -Verb RunAs
        exit
    }
}

function Start-Ollama {
    ollama serve
}

Set-Location -Path $PSScriptRoot
switch ($Command.ToLower()) {
    "install"  { Invoke-Install }
    "console"  { Invoke-Console $Name1 $Name2 }
    "html"     { Invoke-HtmlHost }
    default    {
        Write-Host "Unknown command: $Command"
        exit 1
    }
}

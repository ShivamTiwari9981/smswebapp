param(
    [string]$SiteName = "smswebapp",
    [string]$AppPoolName  = "smswebpool"
)

$site = Get-Website -Name $SiteName -ErrorAction SilentlyContinue

if ($null -ne $site -and $site.state -eq "Started") {
    Write-Output "Stopping site $SiteName..."
    Stop-Website -Name $SiteName
    Stop-WebAppPool -Name $SitePool
} else {
    Write-Output "Site $SiteName is not running, skipping stop."
}

# Copy files
robocopy publish_output "C:\inetpub\wwwroot\$SiteName" /MIR /R:3 /W:5

# Start again
Start-WebAppPool -Name $SitePool
Start-Website -Name $SiteName

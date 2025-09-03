pipeline {
    agent any

    environment {
        SOLUTION = "StudentManagement.sln"
        PROJECT = "StudentManagement/StudentManagement.csproj"
        PUBLISH_DIR = "publish_output"
        IIS_SITE_PATH = "C:\\inetpub\\wwwroot\\smswebapp"
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main',
                    credentialsId: 'github-token',
                    url: 'https://github.com/ShivamTiwari9981/smswebapp.git'
            }
        }

        stage('Restore') {
            steps {
                bat "dotnet restore %SOLUTION%"
            }
        }

        stage('Build') {
            steps {
                bat "dotnet build %SOLUTION% -c Release"
            }
        }

        stage('Test') {
            steps {
                bat "dotnet test %SOLUTION% --no-build -c Release"
            }
        }

        stage('Publish') {
            steps {
                bat "dotnet publish %PROJECT% -c Release -o %PUBLISH_DIR%"
            }
        }

        stage('Deploy to IIS') {
    steps {
        bat '''
        powershell -Command "
        $site = Get-Website -Name 'smswebapp' -ErrorAction SilentlyContinue
        if ($null -ne $site -and $site.state -eq 'Started') {
            Write-Output 'Stopping site smswebapp...'
            Stop-Website -Name 'smswebapp'
            Stop-WebAppPool -Name 'smswebpool'
        } else {
            Write-Output 'Site smswebapp is not running, skipping stop.'
        }
        "
        '''

        bat '''
        robocopy publish_output C:\\inetpub\\wwwroot\\smswebapp /MIR /R:3 /W:5
        '''

        bat '''
        powershell -Command "
        if ($null -ne (Get-Website -Name 'smswebapp' -ErrorAction SilentlyContinue)) {
            Start-WebAppPool -Name 'smswebpool'
            Start-Website -Name 'smswebapp'
        }
        "
        '''
    }
}

    }
}

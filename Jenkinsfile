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
        // Stop site and app pool
        bat '''
        powershell Stop-WebSite -Name "smswebapp"
        powershell Stop-WebAppPool -Name "smswebpool"
        '''

        // Use robocopy instead of xcopy
        bat '''
        robocopy publish_output C:\\inetpub\\wwwroot\\smswebapp /MIR /R:3 /W:5
        '''

        // Restart app pool and site
        bat '''
        powershell Start-WebAppPool -Name "smswebpool"
        powershell Start-WebSite -Name "smswebapp"
        '''
    }
}

    }
}

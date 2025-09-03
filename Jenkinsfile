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
                bat "powershell Stop-WebSite -Name 'smswebapp'"
                bat "xcopy /s /y %PUBLISH_DIR% %IIS_SITE_PATH%"
                bat "powershell Start-WebSite -Name 'smswebapp'"
            }
        }
    }
}

properties([pipelineTriggers([githubPush()])])

pipeline {
    agent any
    environment {
        dotnet ='C:\\Program Files (x86)\\dotnet\\'
        scannerHome = tool 'SonarQube'
    }
    stages {
        stage('Checkout') {
            steps {
                git credentialsId: 'bd7e829d-87f5-4668-aeb8-d24c443b2f3c', url: 'https://github.com/taras-gr/MyFinanceApi.git/', branch: 'master'
            }
        }
        stage('Restore packages'){
            steps{
                bat "dotnet restore MyFinance.Api\\MyFinance.Api.csproj"
            }
        }
        stage('Clean'){
            steps{
                bat "dotnet clean MyFinance.Api\\MyFinance.Api.csproj"
            }
        }
        stage('Build') {
            steps{
                bat "dotnet build MyFinance.Api\\MyFinance.Api.csproj --configuration Release"
            }
        }
        stage('SonarQube analysis') {
            steps {
                echo "${scannerHome}"
                bat "dotnet d:\\Install\\sonar-scanner-msbuild-4.10.0.19059-netcoreapp3.0\\SonarScanner.MSBuild.dll begin /k:mf1"
                bat 'dotnet build MyFinanceApi.sln'
                bat "dotnet d:\\Install\\sonar-scanner-msbuild-4.10.0.19059-netcoreapp3.0\\SonarScanner.MSBuild.dll end"
            }    
        }
        stage('Test: Unit Test'){
            steps {
                bat "dotnet test MyFinance.Api.Tests\\MyFinance.Api.Tests.csproj"
            }
        }
        stage('Docker build') {
            steps {
                script {
                    docker.withRegistry('https://registry.hub.docker.com', 'dockerHub') {
                    def customImage = docker.build('tarik2000/myfinanceapi')

                    customImage.push()
                    }
                }                
            }
        }
        stage('Deploy to IIS') {
            steps {
                script {
                    def msbuild = tool name: 'MSBuild', type: 'hudson.plugins.msbuild.MsBuildInstallation'
                    bat "${msbuild} MyFinanceApi.sln /p:DeployOnBuild=true /p:DeployDefaultTarget=WebPublish /p:WebPublishMethod=FileSystem /p:SkipInvalidConfigurations=true /t:build /p:Configuration=Release /p:Platform=\"Any CPU\" /p:DeleteExistingFiles=True /p:publishUrl=D:\\MyFinanceApi"
                }
            }
        }    

    }
}
pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                // git branch: 'main', url:'https://github.com/dines14-coder/rmkvlocal.git'
                sh "ls -ltr"
            }
        }
        stage('Build Image and Push') {
            environment {
                DOCKER_IMAGE = "dinesh14coder/angular-app:angular${BUILD_NUMBER}"
                REGISTRY_CREDENTIALS = credentials("dock-cred")
            }
            steps {
                script {
                    catchError(buildResult: 'SUCCESS', stageResult: 'FAILURE') {
                        sh 'docker --version'
                        sh 'whoami'
                        sh 'docker build -t ${DOCKER_IMAGE} .'
                        def dockerImage = docker.image("${DOCKER_IMAGE}")
                        withDockerRegistry([credentialsId: 'dock-cred', url: 'https://index.docker.io/v1/']) { 
                            dockerImage.push()
                        }
                    }
                }
            }
        }
        stage('Update Deployment File') {
        environment {
            GIT_REPO_NAME = "rmkvlocal"
            GIT_USER_NAME = "dines14-coder"
        }
        steps {
            withCredentials([string(credentialsId: 'rmkv', variable: 'GITHUB_TOKEN')]) {
                sh '''
                    git config user.email "dvrdineshdvrdinesh728@gmail.com"
                    git config user.name "dines14-coder"
                    REPLACE="angular-app:angular${BUILD_NUMBER}"
                    sed -i "s/angular-app*/${REPLACE}/g" angular-manifest/deployment.yml
                    git add angular-manifest/deployment.yml
                    git commit -m "Update deployment image to version ${BUILD_NUMBER}"
                    git push https://${GITHUB_TOKEN}@github.com/${GIT_USER_NAME}/${GIT_REPO_NAME} HEAD:main
                '''
            }
        }
    }
    }
}

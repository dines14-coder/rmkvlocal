pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main', url:'https://github.com/dines14-coder/rmkvlocal.git'
                sh "ls -ltr"
            }
        }
        stage('Build Image and Push') {
            environment{
                DOCKER_IMAGE = "dinesh14coder/angular-app:angular"
                REGISTRY_CREDENTIALS = credentials("docker-cred")
            }
            steps {
                script{
                    sh 'docker --version'
                    sh 'whoami'
                    sh 'sudo su - jenkins'
                    sh 'whoami'
                    sh 'docker build -t ${DOCKER_IMAGE} .'
                    def dockerImage = docker.image("${DOCKER_IMAGE}")
                    docker.withRegistry('https://index.docker.io/v1','docker-cred') {
                        dockerImage.push() 
                    }
                }
            }
        }
    }
}

#!/bin/bash

function installLlmModel {
    startOllama
    model=$(cat src/model.txt)
    ollama pull "$model" 
}

function installNpmPackages {
    npm install --no-fund
}

function installOllama {
    brew install ollama
}

function invokeConsole {
    publishApp
    startOllama
    node out/console-app.js
}

function invokeHtmlHost {
    publishApp
    startOllama
    node out/html-app.js
}

function invokeInstall {
    installNpmPackages
    installOllama
    installLlmModel
}

function publishApp {
    npx tsc
    cp src/model.txt out/model.txt
}

function startOllama {
    ollama serve || true
}

case "$1" in
    "install") invokeInstall;;
    "console") invokeConsole;;
    "html") invokeHtmlHost;;
    *)
        echo "Unknown command: $1"
        exit 1
        ;;
esac

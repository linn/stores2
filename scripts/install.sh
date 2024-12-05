#!/bin/bash
set -ev

dotnet restore

# upgrade node to latest version
if [ "$CI" ] && [ "$TRAVIS" ]
then 
	source ~/.nvm/nvm.sh; 
	nvm install 20;
	nvm use 20;
fi

cd ./src/Service.Host
npm ci
npm run build
cd ../..

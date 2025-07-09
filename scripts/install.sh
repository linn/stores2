#!/bin/bash
set -ev

dotnet restore

# upgrade node to latest version
if [ "$CI" ] && [ "$TRAVIS" ]
then 
	source ~/.nvm/nvm.sh; 
	nvm install 22;
	nvm use 22;
fi

cd ./src/Service.Host
npm ci
BUILD_ENV=production npm run build
cd ../..

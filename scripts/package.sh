#!/bin/bash
set -ev

# build the client app
cd ./src/Service.Host
npm ci
BUILD_ENV=production npm run build
cd ../..

# build dotnet application
dotnet publish
# dotnet publish ./src/Messaging.Host/ -c release
# dotnet publish ./src/Scheduling.Host/ -c release

# determine which branch this change is from
if [ -n "${GITHUB_HEAD_REF}" ]; then
  # GitHub Actions PR
  GIT_BRANCH=$GITHUB_HEAD_REF
elif [ -n "${GITHUB_REF_NAME}" ]; then
  # GitHub Actions push
  GIT_BRANCH=$GITHUB_REF_NAME
fi

# create docker image(s)
docker login -u $DOCKER_HUB_USERNAME -p $DOCKER_HUB_PASSWORD

# Use continuous build number (Travis + GitHub Actions)
LAST_TRAVIS_BUILD_NUMBER="${LAST_TRAVIS_BUILD_NUMBER:-0}"
BUILD_NUMBER=$((LAST_TRAVIS_BUILD_NUMBER + GITHUB_RUN_NUMBER))

docker build --no-cache -t linn/stores2:$BUILD_NUMBER --build-arg gitBranch=$GIT_BRANCH ./src/Service.Host/
# docker build --no-cache -t linn/stores2-messaging:$BUILD_NUMBER --build-arg gitBranch=$GIT_BRANCH ./src/Messaging.Host/
# docker build --no-cache -t linn/stores2-scheduling:$BUILD_NUMBER --build-arg gitBranch=$GIT_BRANCH ./src/Scheduling.Host/

# push to dockerhub 
docker push linn/stores2:$BUILD_NUMBER
# docker push linn/stores2-messaging:$BUILD_NUMBER
# docker push linn/stores2-scheduling:$BUILD_NUMBER

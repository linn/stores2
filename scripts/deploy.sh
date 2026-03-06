#!/bin/bash
set -ev

echo "Checking AWS CLI version..."
aws --version

# deploy on aws
# Handle GitHub Actions environment variables
if [ -n "${GITHUB_REF_NAME}" ]; then
  # Running in GitHub Actions
  CURRENT_BRANCH="${GITHUB_REF_NAME}"
  if [ "${GITHUB_EVENT_NAME}" = "pull_request" ]; then
    IS_PULL_REQUEST="true"
  else
    IS_PULL_REQUEST="false"
  fi
fi

# deploy on aws
if [ "${TRAVIS_BRANCH}" = "main" ]; then
  if [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
    # master - deploy to production
    echo deploy to production

    aws s3 cp s3://$S3_BUCKET_NAME/stores2/production.env ./secrets.env

    STACK_NAME=stores2
    LEGACY_AUTHORITY_URI=https://www.linn.co.uk/auth/
    APP_ROOT=http://app.linn.co.uk
    PROXY_ROOT=http://app.linn.co.uk
  	ENV_SUFFIX=
  else
    # pull request based on master - deploy to sys
    echo deploy to sys

    aws s3 cp s3://$S3_BUCKET_NAME/stores2/sys.env ./secrets.env

    STACK_NAME=stores2-sys
    LEGACY_AUTHORITY_URI=https://www-sys.linn.co.uk/auth/
    APP_ROOT=http://app-sys.linn.co.uk
    PROXY_ROOT=http://app.linn.co.uk
    ENV_SUFFIX=-sys
  fi
else
  # not main - deploy to int if required
  echo do not deploy to int
fi

# load the secret variables but hide the output from the log
source ./secrets.env > /dev/null 2>&1

# deploy the service to amazon
# use continuous build number (Travis + GitHub Actions)
LAST_TRAVIS_BUILD_NUMBER="${LAST_TRAVIS_BUILD_NUMBER:-0}"
BUILD_NUMBER=$((LAST_TRAVIS_BUILD_NUMBER + GITHUB_RUN_NUMBER))

# deploy the service to amazon
aws cloudformation deploy --stack-name $STACK_NAME --template-file ./aws/application.yml --parameter-overrides dockerTag=$BUILD_NUMBER databaseHost=$DATABASE_HOST databaseName=$DATABASE_NAME databaseUserId=$DATABASE_USER_ID databasePassword=$DATABASE_PASSWORD rabbitServer=$RABBIT_SERVER rabbitPort=$RABBIT_PORT rabbitUsername=$RABBIT_USERNAME rabbitPassword=$RABBIT_PASSWORD appRoot=$APP_ROOT proxyRoot=$PROXY_ROOT legacyAuthorityUri=$LEGACY_AUTHORITY_URI cognitoHost=$COGNITO_HOST cognitoClientId=$COGNITO_CLIENT_ID cognitoDomainPrefix=$COGNITO_DOMAIN_PREFIX entraLogoutUri=$ENTRA_LOGOUT_URI viewsRoot=$VIEWS_ROOT pdfServiceRoot=$PDF_SERVICE_ROOT environmentSuffix=$ENV_SUFFIX --capabilities=CAPABILITY_IAM

echo "deploy complete"

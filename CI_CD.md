# GitHub Actions Migration

## Overview
This project implements CI and CD using github actions as follows:

## Workflow
- **Build & Test**: Runs on all pushes and pull requests to `main`
- **Deploy**: Runs after successful build, only for:
  - Pushes to `main` → **Production** environment
  - Pull requests to `main` → **Sys** environment

## Required Secrets
Set these in your GitHub repository settings (Settings → Secrets and variables → Actions):

### Docker Hub
- `DOCKER_HUB_USERNAME`: Docker Hub username
- `DOCKER_HUB_PASSWORD`: Docker Hub password/token

### AWS
- `AWS_ACCESS_KEY_ID`: AWS access key
- `AWS_SECRET_ACCESS_KEY`: AWS secret key  
- `AWS_REGION`: AWS region (optional, defaults to eu-west-1)

### S3 Configuration
- `S3_BUCKET_NAME`: S3 bucket containing environment configuration files

## Environment Files
The deployment expects these files in S3:
- `s3://{S3_BUCKET_NAME}/production2/production.env` - Production environment variables
- `s3://{S3_BUCKET_NAME}/production2/sys.env` - Sys environment variables

## Compatibility
The scripts have been updated to work with both GitHub Actions and Travis CI environment variables for a smooth transition.

## Workflow File
`.github/workflows/ci-cd.yml` - Main CI/CD pipeline
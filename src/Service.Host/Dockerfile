﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0

ARG gitBranch=unspecified
ENV TZ=Europe/London

RUN apt-get update && apt-get install -y curl
RUN rm -rf /var/lib/apt/lists/*

EXPOSE 5050

COPY bin/release/net9.0/publish/ /app/bin/
COPY client/build/ /app/client/build/
COPY views/ /app/views/
     
ENV APP_PATH /app/
ENV GIT_BRANCH $gitBranch

CMD dotnet /app/bin/Linn.Stores2.Service.Host.dll
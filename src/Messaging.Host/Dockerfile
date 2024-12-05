FROM mcr.microsoft.com/dotnet/runtime:8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0

COPY bin/release/net8.0/publish/ /app/bin/
ENV TZ=Europe/London

CMD dotnet /app/bin/Linn.Finance.Messaging.Host.dll
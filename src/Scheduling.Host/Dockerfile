FROM mcr.microsoft.com/dotnet/runtime:8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0

ENV TZ=Europe/London

COPY /bin/release/net8.0/publish/ /app/bin/

CMD dotnet /app/bin/Linn.Stores2.Scheduling.Host.dll

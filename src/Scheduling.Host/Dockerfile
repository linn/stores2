FROM mcr.microsoft.com/dotnet/aspnet:10.0

ENV TZ=Europe/London

COPY /bin/release/net10.0/publish/ /app/bin/

CMD dotnet /app/bin/Linn.Stores2.Scheduling.Host.dll

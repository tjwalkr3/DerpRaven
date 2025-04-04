FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /App
COPY . .

CMD ["dotnet", "test", "DerpRaven.UnitTests"]
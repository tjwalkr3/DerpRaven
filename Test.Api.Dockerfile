FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /App
COPY . .

# RUN dotnet build -warnaserror "DerpRaven.Api/DerpRaven.Api.csproj"

CMD ["dotnet", "test", "DerpRaven.NTests"]
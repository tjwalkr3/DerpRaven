# Prepare the projects
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /App
COPY . ./
RUN dotnet restore "DerpRaven.Api/DerpRaven.Api.csproj"
RUN dotnet publish "DerpRaven.Api/DerpRaven.Api.csproj" -c Release -o publish

# Build the image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build /App/publish .
ENTRYPOINT ["dotnet", "DerpRaven.Api/DerpRaven.Api.dll"]
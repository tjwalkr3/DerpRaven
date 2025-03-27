# Prepare the projects
ARG base_address
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /App
COPY . ./
RUN dotnet restore "DerpRaven.Web/DerpRaven.Web.csproj"
RUN dotnet publish "DerpRaven.Web/DerpRaven.Web.csproj" -c Release -o publish

# Inject the base address into the appsettings.json
RUN echo "{ \"BaseAddress\": \"${base_address}\" }" > /App/publish/wwwroot/appsettings.json

# Use NGINX to serve the static files
FROM nginx:latest AS final
COPY --from=build /App/publish/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
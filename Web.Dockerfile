# Prepare the projects
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BASE_ADDRESS
ARG FEATURE_FLAG_ON

WORKDIR /App
COPY . ./
RUN dotnet restore "DerpRaven.Web/DerpRaven.Web.csproj"
RUN dotnet publish "DerpRaven.Web/DerpRaven.Web.csproj" -c Release -o publish

# Inject the base address into the appsettings.json
RUN echo "{\n    \"BaseAddress\": \"${BASE_ADDRESS}\",\n    \"FeatureFlagEnabled\": \"${FEATURE_FLAG_ON}\"}" \
    > /App/publish/wwwroot/appsettings.json

# Use NGINX to serve the static files
FROM nginx:latest AS final

# Configure nginx for relative routing
RUN rm /etc/nginx/conf.d/default.conf
COPY ./config/nginx.conf /etc/nginx/conf.d

# Copy the WASM files to the correct server directory
COPY --from=build /App/publish/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
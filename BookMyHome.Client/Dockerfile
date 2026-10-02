FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY BookMyHome.Client/BookMyHome.Client.csproj BookMyHome.Client/
RUN dotnet restore BookMyHome.Client/BookMyHome.Client.csproj

COPY . .
RUN dotnet publish BookMyHome.Client/BookMyHome.Client.csproj \
    -c Release -o /app/publish --no-restore

FROM nginx:alpine AS final
LABEL org.opencontainers.image.source="https://github.com/Morina8700/Nookly"
COPY --from=build /app/publish/wwwroot/ /usr/share/nginx/html/
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80

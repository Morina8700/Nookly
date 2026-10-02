FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY BookMyHome.Application/BookMyHome.Application.csproj BookMyHome.Application/
COPY BookMyHome.Persistence/BookMyHome.Persistence.csproj BookMyHome.Persistence/
COPY BookMyHome.Domain/BookMyHome.Domain.csproj BookMyHome.Domain/

RUN dotnet restore BookMyHome.Application/BookMyHome.Application.csproj

COPY . .
RUN dotnet publish BookMyHome.Application/BookMyHome.Application.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
LABEL org.opencontainers.image.source="https://github.com/Morina8700/Nookly"
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookMyHome.Application.dll"]

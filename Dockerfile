FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

RUN apt-get update \
    && apt-get install -y curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
EXPOSE 8001
ENV ASPNETCORE_URLS=http://+:8001

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY BookMyHome.Application/BookMyHome.Application.csproj BookMyHome.Application/
COPY BookMyHome.Persistence/BookMyHome.Persistence.csproj BookMyHome.Persistence/
COPY BookMyHome.Domain/BookMyHome.Domain.csproj BookMyHome.Domain/

RUN dotnet restore "BookMyHome.Application/BookMyHome.Application.csproj"

COPY BookMyHome.Application/ BookMyHome.Application/
COPY BookMyHome.Persistence/ BookMyHome.Persistence/
COPY BookMyHome.Domain/ BookMyHome.Domain/

RUN dotnet build "BookMyHome.Application/BookMyHome.Application.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/build

FROM build AS publish

RUN dotnet publish "BookMyHome.Application/BookMyHome.Application.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

FROM base AS final

WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "BookMyHome.Application.dll"]

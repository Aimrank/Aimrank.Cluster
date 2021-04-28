FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /app

COPY *.sln .
COPY src/Aimrank.Cluster.Api/*.csproj ./src/Aimrank.Cluster.Api/
COPY src/Aimrank.Cluster.Core/*.csproj ./src/Aimrank.Cluster.Core/
COPY src/Aimrank.Cluster.Infrastructure/*.csproj ./src/Aimrank.Cluster.Infrastructure/

RUN dotnet restore

COPY src/Aimrank.Cluster.Api/. ./src/Aimrank.Cluster.Api/
COPY src/Aimrank.Cluster.Core/. ./src/Aimrank.Cluster.Core/
COPY src/Aimrank.Cluster.Infrastructure/. ./src/Aimrank.Cluster.Infrastructure/

RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app

COPY --from=build /app/out/ .

RUN apt-get update && apt-get install -y curl

HEALTHCHECK --interval=30s --timeout=30s --start-period=30s --retries=5 \
  CMD curl -f http://localhost/ || exit 1
  
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["Aimrank.Cluster.Api.dll"]
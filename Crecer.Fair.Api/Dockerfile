# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Crecer.Fair.Api.sln .
COPY Crecer.Fair.Api/*.csproj ./Crecer.Fair.Api/
COPY Crecer.Fair.Data/*.csproj ./Crecer.Fair.Data/
COPY Crecer.Fair.Infrastructure/*.csproj ./Crecer.Fair.Infrastructure/
RUN dotnet restore

# copy everything else and build app
COPY Crecer.Fair.Api/. ./Crecer.Fair.Api
COPY Crecer.Fair.Data/. ./Crecer.Fair.Data
COPY Crecer.Fair.Infrastructure/. ./Crecer.Fair.Infrastructure
WORKDIR /source/Crecer.Fair.Api
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Crecer.Fair.Api.dll"]
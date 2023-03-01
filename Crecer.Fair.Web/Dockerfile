# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Crecer.Fair.Web.sln .
COPY Crecer.Fair.Web/*.csproj ./Crecer.Fair.Web/
RUN dotnet restore

# copy everything else and build app
COPY Crecer.Fair.Web/. ./Crecer.Fair.Web/
WORKDIR /source/Crecer.Fair.Web
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Crecer.Fair.Web.dll"]
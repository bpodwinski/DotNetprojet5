# Utiliser l'image de base .NET Core SDK pour la compilation
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copier les fichiers csproj et restaurer les d�pendances
COPY *.csproj ./
RUN dotnet restore

# Copier tous les autres fichiers et construire l'application
COPY . ./
RUN dotnet publish -c Release -o out

# Utiliser l'image de base .NET Core Runtime pour ex�cuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Exposer le port que l'application �coute
EXPOSE 80
EXPOSE 443

# D�marrer l'application
ENTRYPOINT ["dotnet", "ExpressVoitures.dll"]

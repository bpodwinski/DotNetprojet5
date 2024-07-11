FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0 AS build
WORKDIR /app

RUN apt-get update \
    && apt-get install -y libgssapi-krb5-2

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/nightly/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
EXPOSE 8443

ENTRYPOINT ["dotnet", "ExpressVoitures.dll"]

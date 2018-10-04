FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src

## install OS level packages
# RUN apt-get update && apt-get install -y unzip jq

## deploy zipfile with application code
COPY *.csproj ./

## restore dotnet packages
RUN dotnet restore -nowarn:msb3202,nu1503

## build the code
COPY . ./
RUN dotnet build -c Release -o /app

## publish built code
FROM build AS publish
RUN dotnet publish -c Release -o /app

## configure code for edge DB
COPY appsettings.json /app/

## deploy published code to final container
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DynoCardWebAPI.dll"]


# FROM microsoft/dotnet:2.1-sdk AS build-env
# WORKDIR /app

# COPY *.csproj ./
# RUN dotnet restore

# COPY . ./
# RUN dotnet publish -c Release -o out

# FROM microsoft/dotnet:2.1-aspnetcore-runtime
# WORKDIR /app
# COPY --from=build-env /app/out ./
# EXPOSE 80

# ENTRYPOINT ["dotnet", "DynoCardWebAPI.dll"]
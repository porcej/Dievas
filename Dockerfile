# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY Dievas.csproj .
RUN dotnet restore

COPY . .

# Support clean clones that did not initialize git submodules.
RUN apt-get update \
    && apt-get install -y --no-install-recommends git \
    && if [ ! -f Models/Dashboard.Models/UnitDto.cs ]; then \
         git clone --depth 1 https://github.com/timbellomo/Dashboard.Models.git Models/Dashboard.Models; \
       fi \
    && rm -rf /var/lib/apt/lists/*

RUN dotnet publish Dievas.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Dievas.dll", "--urls", "http://+:8080"]

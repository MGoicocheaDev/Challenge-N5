#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["web-api-permision/*.csproj", "web-api-permision/"]
COPY ["web-api-lib-application/*.csproj", "web-api-lib-application/"]
COPY ["web-api-lib-data/*.csproj", "web-api-lib-data/"]
RUN dotnet restore "web-api-permision/web-api-permision.csproj"
COPY ["web-api-permision/", "web-api-permision/"]
COPY ["web-api-lib-application/", "web-api-lib-application/"]
COPY ["web-api-lib-data/", "web-api-lib-data/"]
WORKDIR "/src/"
RUN dotnet build "web-api-permision/web-api-permision.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web-api-permision/web-api-permision.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "web-api-permision.dll"]
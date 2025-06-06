﻿FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["matchMaker.Services/Profile/Profile.API/Profile.API.csproj", "matchMaker.Services/Profile/Profile.API/"]
RUN dotnet restore "matchMaker.Services/Profile/Profile.API/Profile.API.csproj"
COPY . .
WORKDIR "/src/matchMaker.Services/Profile/Profile.API"
RUN dotnet build "Profile.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Profile.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profile.API.dll"]

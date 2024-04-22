#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ConnectionStrings__CathaybkHWDB="Testdddd"

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CathaybkHW/CathaybkHW.csproj", "CathaybkHW/"]
COPY ["CathaybkHW.Application/CathaybkHW.Application.csproj", "CathaybkHW.Application/"]
COPY ["CathaybkHW.Domain/CathaybkHW.Domain.csproj", "CathaybkHW.Domain/"]
COPY ["CathaybkHW.Infrastructure/CathaybkHW.Infrastructure.csproj", "CathaybkHW.Infrastructure/"]
RUN dotnet restore "./CathaybkHW/CathaybkHW.csproj"
COPY . .
WORKDIR "/src/CathaybkHW"
RUN dotnet build "./CathaybkHW.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CathaybkHW.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CathaybkHW.dll"]

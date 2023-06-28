FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["other/IdentityService/IdentityService.csproj", "other/IdentityService/"]
RUN dotnet restore "other/IdentityService/IdentityService.csproj"
COPY . .
WORKDIR "/src/other/IdentityService"
RUN dotnet build "IdentityService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.dll"]

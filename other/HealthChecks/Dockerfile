FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["other/HealthChecks/HealthChecks.csproj", "other/HealthChecks/"]
RUN dotnet restore "other/HealthChecks/HealthChecks.csproj"
COPY . .
WORKDIR "/src/other/HealthChecks"
RUN dotnet build "HealthChecks.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthChecks.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthChecks.dll"]

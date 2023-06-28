FROM mcr.microsoft.com/dotnet/aspnet:8.0-preview AS base
WORKDIR /app

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["src/Worker/Worker.csproj", "src/Worker/"]
RUN dotnet restore "src/Worker/Worker.csproj"
COPY . .
WORKDIR "/src/src/Worker"
RUN dotnet build "Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]

FROM mcr.microsoft.com/dotnet/sdk:8.0-preview AS build
WORKDIR /src
COPY ["src/ClientApp/ClientApp.csproj", "src/ClientApp/"]
RUN dotnet restore "src/ClientApp/ClientApp.csproj"
COPY . .
WORKDIR "/src/src/ClientApp"
RUN dotnet build "ClientApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClientApp.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .

RUN chmod -R 644 _framework/*.wasm

COPY nginx.conf /etc/nginx/nginx.conf
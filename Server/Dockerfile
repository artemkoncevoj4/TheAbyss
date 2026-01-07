FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта (ищем его рекурсивно, чтобы не ошибиться с папкой)
COPY Server/GameServer/GameServer.csproj ./GameServer/
RUN dotnet restore "GameServer/GameServer.csproj"

# Копируем всё остальное
COPY Server/ .

# Собираем
RUN dotnet publish "GameServer/GameServer.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
EXPOSE 7777/udp

ENTRYPOINT ["dotnet", "GameServer.dll"]
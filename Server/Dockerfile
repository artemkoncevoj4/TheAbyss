FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем файл проекта из подпапки
COPY ["GameServer/GameServer.csproj", "GameServer/"]

# Восстанавливаем зависимости
RUN dotnet restore "GameServer/GameServer.csproj"

# Копируем всё остальное
COPY . .

# Собираем и публикуем (убрали --no-restore для надежности)
RUN dotnet publish "GameServer/GameServer.csproj" -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80
EXPOSE 7777/udp

ENTRYPOINT ["dotnet", "GameServer.dll"]
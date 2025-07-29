# Weather MCP Server

## Установка и настройка

### 1. Предварительные требования
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- API ключ от [OpenWeatherMap](https://openweathermap.org/api)

### 2. Настройка окружения

#### Вариант A: Через переменные окружения
```bash
# Windows
setx OpenWeatherMap__ApiKey "your_api_key"

# Linux/macOS
export OpenWeatherMap__ApiKey="your_api_key"
```

#### Вариант B: Через файл конфигурации

1. Откройте файл `appsettings.json` в корне проекта
2. Добавьте следующий конфигурационный блок:

```json
{
  "OpenWeatherMap": {
    "ApiKey": "your_api_key_here",
  }

```

### 3. Запуск сервера

```bash
dotnet run
```


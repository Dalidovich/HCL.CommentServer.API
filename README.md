# HCL.CommentServer.API

## Описание проекта

CommentServer.API - это серверная часть системы комментариев, разработанная на .NET 7. Проект предоставляет RESTful API для управления комментариями, интегрирует SignalR для реального времени и использует PostgreSQL в качестве основной базы данных.

## Основные функциональные возможности

1. **Управление комментариями**:
   - Создание, чтение, обновление и удаление комментариев
   - Оценка комментариев (хороший/плохой/нормальный)

2. **Real-time взаимодействие**:
   - Отправка комментариев в реальном времени через SignalR
   - Управление подключениями пользователей через чат-менеджер

3. **Авторизация и аутентификация**:
   - JWT-аутентификация
   - Ролевая модель (пользователь/администратор)

4. **Логирование**:
   - Интеграция с Elasticsearch для централизованного логирования
   - Подробные логи операций

## Технологический стек

- **Язык**: C#
- **Фреймворк**: .NET 7
- **База данных**: PostgreSQL
- **Real-time**: SignalR
- **Логирование**: Serilog + Elasticsearch
- **Контейнеризация**: Docker
- **Оркестрация**: Kubernetes (опционально)
- **API документация**: Swagger

## Структура проекта
HCL.CommentServer.API/  
├── BLL/ # Бизнес-логика  
│ ├── Hubs/ # SignalR хабы  
│ ├── Interfaces/ # Интерфейсы сервисов  
│ └── Services/ # Реализации сервисов  
├── DAL/ # Доступ к данным  
│ ├── Configuration/ # Конфигурации Entity Framework  
│ ├── Migrations/ # Миграции базы данных  
│ ├── Repositories/ # Репозитории  
│ └── Contexts/ # Контексты БД  
├── Domain/ # Доменные модели  
│ ├── DTO/ # Data Transfer Objects  
│ ├── Entities/ # Сущности  
│ ├── Enums/ # Перечисления  
│ └── InnerResponse/ # Внутренние модели ответов  
├── Middleware/ # Пользовательские middleware  
├── Test/ # Тесты  
├── Program.cs # Точка входа  
└── appsettings.json # Конфигурация приложения  


## Настройка окружения

### Требования

- .NET 7 SDK
- Docker (для контейнеризации)
- PostgreSQL 14+
- Elasticsearch 7.16.1 (опционально для логирования)

### Конфигурация

1. **База данных**:
   - Настройки подключения в `appsettings.json`:
     ```json
     "ConnectionStrings": {
       "NpgConnectionString": "User Id=postgres; Password=pg; Server=localhost; Port=5432; Database=HCL_Comment; IntegratedSecurity=true; Pooling=true;"
     }
     ```

2. **JWT**:
   - Настройте секретный ключ и параметры в `appsettings.json`:
     ```json
     "JWTSettings": {
       "SecretKey": "your-secret-key",
       "Issuer": "your-issuer",
       "Audience": "your-audience"
     }
     ```

3. **Elasticsearch**:
   - Укажите URL Elasticsearch в переменных окружения или `appsettings.json`:
     ```json
     "ElasticConfiguration": {
       "Uri": "http://localhost:9200"
     }
     ```

## API Endpoints
### Комментарии
- **`GET /odata/v1/comment`** - Получение списка комментариев (OData)
- **`DELETE /api/comment/v1/comment/account`** - Удаление комментария (для владельца)
- **`DELETE /api/comment/v1/comment/admin`** - Удаление комментария (для администратора)
### SignalR
- **`/comment`** - SignalR хаб для real-time взаимодействия

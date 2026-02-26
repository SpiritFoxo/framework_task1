# Технологии разработки приложений на базе фреймворков

Минимальный веб-сервис на .NET 8 для учёта учебных дисциплин.  
Хранит данные в памяти процесса, предоставляет REST API и красивый конвейер обработки запросов.

## Архитектурная идея

- **Конвейер middleware** (RequestId → TimingAndLog → ErrorHandling) — каждый слой делает ровно одну вещь и не мешает остальным.
- **Единый формат ошибок** (`ErrorResponse`) с `requestId` для трассировки.
- **Доменная модель** через `record` + исключения `DomainException`.
- **Маршрутизация** через MapGet/MapPost + внедрение зависимостей.
- **Хранение** абстрагировано через интерфейс репозитория.

## Деплой 

```bash
# 1. Клонирование + сборка + запуск
git clone https://github.com/твой-логин/framework_task1.git
cd framework_task1
docker build -t discipline-service .
docker run -d --name discipline-app -p 54254:54254 discipline-service
```

## Curl запросы
### 1. Создание дисциплины

```bash
curl -X POST http://localhost:8080/api/disciplines \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Алгоритмы и структуры данных",
    "lecturer": "Иванов Иван Иванович",
    "studentCount": 87
  }'
```

### 2. Получить список всех дисциплин


```bash
curl http://localhost:8080/api/disciplines
```

### 3. Получить дисциплину по ID

```bash
curl http://localhost:8080/api/disciplines/{id}
```

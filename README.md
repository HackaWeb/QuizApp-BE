# QuizApp-BE

**QuizApp** - це веб-застосунок для створення та проходження квізів. Він включає аутентифікацію, авторизацію, завантаження медіафайлів у Azure Blob Storage та підтримку контейнеризації з Docker.


🌐 Хостинг

Додаток уже розгорнутий та доступна документація Swagger. Перейдіть за посиланням: 

https://hackawebquiz.ashycoast-bbbe20af.westus2.azurecontainerapps.io/swagger/index.html

📌 Функціональні можливості

- Реєстрація та авторизація користувачів з використанням ASP.NET Core Identity та JWT

- Створення, редагування та видалення квізів

- Додавання зображень і відео до запитань через Azure Blob Storage

- Збереження даних у PostgreSQL

- Контейнеризація з Docker і Docker Compose

- CI/CD для автоматичного розгортання на Azure

- Валідація вхідних запитів за допомогою FluentValidation

🏗️ Технології

- Backend: ASP.NET Core, MediatR, EF Core, PostgreSQL

- Storage: Azure Blob Storage

- Security: ASP.NET Core Identity, JWT

- Infrastructure: Docker, Docker Compose, Azure
- Validation: FluentValidation

🚀 Запуск локально

**1. Клонування репозиторію**

```
git clone https://github.com/your-repo/QuizApp.git
cd QuizApp
```

**2. Налаштування змінних середовища**

Створи файл .env і додай до нього:

```
DB_CONNECTION_STRING=Server=localhost;Port=5432;Database=quizdb;User Id=postgres;Password=yourpassword;
AZURE_BLOB_STORAGE_CONNECTION_STRING=your_connection_string
JWT_SECRET=your_secret_key
```

**3. Запуск бази даних за допомогою Docker**
```
docker-compose up -d
```
**4. Запуск застосунку**
```
dotnet run
```

🐳 Розгортання в Docker

1. Зібрати та запустити контейнери
```
docker-compose up --build
```
🔑 Аутентифікація

Застосунок використовує JWT-токени для авторизації.

Приклад запиту на логін:

```
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "yourpassword"
}
```
Відповідь:
```
{
  "token": "your_jwt_token"
}
```
Цей токен потім передається в заголовку Authorization: Bearer your_jwt_token у захищених запитах.


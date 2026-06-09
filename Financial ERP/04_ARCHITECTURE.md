# Architecture

## Architectural Style

Modular Monolith

Причины:

* Быстрая разработка MVP
* Простота поддержки одним разработчиком
* Отсутствие сетевых накладных расходов между сервисами
* Простое тестирование
* Возможность последующего выделения модулей в микросервисы

---

## Technology Stack

### Frontend

Blazor Server

Причины:

* Один язык (C#)
* Быстрая разработка интерфейсов
* Минимизация контекста переключения между фронтендом и бэкендом
* Подходит для ERP/CRM систем с большим количеством таблиц и форм

---

### Backend

ASP.NET Core 9

Архитектурный подход:

* Clean Architecture
* CQRS (только там, где действительно нужен)
* Dependency Injection

---

### Database

PostgreSQL

Причины:

* Надёжность
* Производительность
* Отличная поддержка аналитических запросов
* Поддержка JSONB
* Хорошая масштабируемость

---

### ORM

Entity Framework Core

---

### Validation

FluentValidation

---

### Authentication

JWT

Дополнительно в будущем:

* Refresh Tokens
* Two-Factor Authentication

---

### File Storage

S3 Compatible Storage

Варианты:

* MinIO
* AWS S3
* Cloudflare R2

---

### Caching

В MVP отсутствует.

После появления нагрузки:

* Redis

---

### Logging

Serilog

Хранение:

* PostgreSQL
* Файлы

---

### Background Jobs

В MVP:

* Hosted Services

Позже:

* Hangfire

---

### Containerization

Docker

Для:

* API
* PostgreSQL
* MinIO

---

## Solution Structure

FinancialERP.sln

FinancialERP.Domain

FinancialERP.Application

FinancialERP.Infrastructure

FinancialERP.Persistence

FinancialERP.Api

FinancialERP.Web

FinancialERP.Tests

---

## Layer Responsibilities

### Domain

Содержит:

* Entities
* Value Objects
* Domain Rules
* Enums

Не зависит ни от чего.

---

### Application

Содержит:

* Use Cases
* Services
* DTO
* Validators

Не знает о БД.

---

### Infrastructure

Содержит:

* Email
* File Storage
* Integrations
* External Services

---

### Persistence

Содержит:

* DbContext
* Entity Configurations
* Repositories
* Migrations

---

### Api

Содержит:

* Controllers
* Authorization
* OpenAPI

---

### Web

Blazor UI.

Содержит:

* Pages
* Components
* Forms
* Dashboards

---

## Multitenancy

Каждая бизнес-сущность содержит:

OrganizationId

Фильтрация данных обязательна на уровне приложения.

Запрещено получать данные другой организации.

---

## Audit

Каждое изменение должно сохраняться.

Минимальный набор:

* Кто изменил
* Когда изменил
* Что изменил
* Старое значение
* Новое значение
* Причина изменения

---

## Development Principles

1. Сначала бизнес-логика.
2. Потом интерфейс.
3. Сначала рабочий процесс.
4. Потом оптимизация.
5. Никаких микросервисов до появления реальной нагрузки.
6. Никаких преждевременных интеграций.
7. Любая новая функция должна приносить бизнес-ценность.

# BOUNDED CONTEXTS

## Identity

Отвечает за:

* User
* Role
* Permission
* Authentication
* Authorization

Не хранит:

* сделки
* проекты
* финансы

---

## Organization

Отвечает за:

* Organization
* Departments
* Company Settings

Каждая сущность системы обязана принадлежать Organization.

---

## CRM

Отвечает за:

* Client
* Lead

CRM заканчивается после согласования лида.

CRM не управляет финансами.

CRM не управляет проектами.

---

## Sales

Отвечает за:

* Deal
* Deal Status
* Deal Documents

Получает данные из Lead.

Отвечает за коммерческую часть сделки.

---

## Project Management

Отвечает за:

* Project
* Project Costs
* Project Progress

Не управляет клиентами.

Не управляет пользователями.

---

## Finance

Отвечает за:

* FinanceEntry
* AdminExpense
* Profitability
* Margin
* Forecasts

Является единственным источником финансовой правды.

Любые финансовые расчёты должны проходить через этот контекст.

---

## Workflow

Отвечает за:

* WorkflowTemplate
* WorkflowInstance
* WorkflowStep

Не хранит бизнес-данные.

Хранит только процессы.

---

## Tasks

Отвечает за:

* Task
* Notifications

Не хранит финансовую информацию.

---

## Warehouse

Отвечает за:

* Products
* Warehouses
* Reservations
* Shipments

Не знает ничего о расчётах рентабельности.

Передаёт данные в Finance.

---

## Audit

Отвечает за:

* AuditLog

Никогда не удаляется.

Любая критическая операция должна создавать запись аудита.

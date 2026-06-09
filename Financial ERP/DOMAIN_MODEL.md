# DOMAIN MODEL

## PRINCIPLE

Система строится вокруг прибыли.

Не вокруг клиентов.

Не вокруг проектов.

Не вокруг задач.

Главная бизнес-сущность системы:

Контрактная деятельность компании.

---

# AGGREGATES

## Organization Aggregate

Root:

Organization

Children:

* Department
* OrganizationSettings
* OrganizationCurrency
* OrganizationTaxSettings

Responsibilities:

* Изоляция данных
* Настройки компании
* Базовые справочники

---

## User Aggregate

Root:

User

Children:

* UserRole
* UserDepartment

Responsibilities:

* Авторизация
* Доступы
* Назначения

---

## Client Aggregate

Root:

Client

Responsibilities:

* Контрагент
* История взаимодействий

Client не хранит сделки.

Client не хранит проекты.

---

## Lead Aggregate

Root:

Lead

Children:

* LeadCalculation
* LeadVersion
* LeadApproval

Responsibilities:

* Первичный расчёт
* Согласование
* Оценка прибыльности

State Machine:

Draft

Submitted

FinanceReview

DirectorReview

Approved

Rejected

Archived

Business Rules:

Approved Lead нельзя редактировать.

Изменение создаёт новую версию.

---

## Deal Aggregate

Root:

Deal

Children:

* DealItem
* DealDocument
* DealRevision

Responsibilities:

* Продажа товара
* Контроль исполнения сделки

Statuses:

Created

InProgress

Completed

Failed

Archived

---

## Project Aggregate

Root:

Project

Children:

* ProjectCost
* ProjectStage
* ProjectRevision

Responsibilities:

* Оказание услуг
* Выполнение работ

Statuses:

Planning

Approved

InProgress

Completed

Cancelled

Archived

---

## Finance Aggregate

Root:

FinanceEntry

Children:

* FinanceAdjustment
* FinanceApproval

Responsibilities:

* Единственный источник финансовой правды

Через этот агрегат проходят:

* доходы
* расходы
* налоги
* корректировки

Запрещено хранить финансовую истину внутри Deal.

Запрещено хранить финансовую истину внутри Project.

---

## Task Aggregate

Root:

Task

Children:

* TaskComment
* TaskAttachment

Responsibilities:

* Исполнение поручений

---

## Workflow Aggregate

Root:

WorkflowInstance

Children:

* WorkflowStep
* WorkflowHistory

Responsibilities:

* Согласования
* Бизнес-процессы

---

## Warehouse Aggregate

Root:

Warehouse

Children:

* StockItem
* Reservation
* Shipment

Responsibilities:

* Учёт остатков

Не отвечает за прибыльность.

Не отвечает за финансы.

---

## Audit Aggregate

Root:

AuditLog

Responsibilities:

* История действий

Никогда не изменяется.

Никогда не удаляется.

Append Only.

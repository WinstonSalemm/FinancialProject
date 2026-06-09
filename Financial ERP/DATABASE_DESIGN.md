# DATABASE DESIGN

## GLOBAL RULES

Каждая бизнес-таблица содержит:

Id

OrganizationId

CreatedAt

CreatedBy

UpdatedAt

UpdatedBy

DeletedAt

Version

---

## AUDITABLE TABLES

Обязательный аудит:

Lead

Deal

Project

FinanceEntry

AdminExpense

User

Role

Department

WorkflowInstance

Task

---

## SOFT DELETE

Запрещено физическое удаление:

* Deal
* Project
* FinanceEntry
* AuditLog

Допускается:

DeletedAt

IsArchived

---

## MONEY RULES

Запрещено использовать:

float

double

Для денег использовать:

decimal(18,2)

или

decimal(18,4)

---

## DATE RULES

Все даты:

UTC

timestamp with time zone

---

## CURRENCY RULES

Хранить:

Amount

CurrencyCode

ExchangeRate

BaseCurrencyAmount

Нельзя хранить только Amount.

---

## CONCURRENCY

Каждая критичная таблица:

RowVersion

Optimistic Concurrency

---

## INDEX RULES

Обязательные индексы:

OrganizationId

CreatedAt

Status

ClientId

AssignedTo

---

## REPORTING

Запрещено строить отчёты напрямую из UI.

Все KPI рассчитываются сервером.

Источник:

FinanceEntry

AdminExpense

Deal

Project

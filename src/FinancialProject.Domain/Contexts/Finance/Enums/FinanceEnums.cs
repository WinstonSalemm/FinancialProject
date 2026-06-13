namespace FinancialProject.Domain.Contexts.Finance.Enums;

public enum FinanceEntryType
{
    Revenue = 1,
    Expense = 2,
    Salary = 3,
    Tax = 4,
    Rent = 5,
    Utility = 6,
    Logistics = 7,
    Administrative = 8,
    Other = 9,
    Adjustment = 10
}

public enum FinanceFlowDirection
{
    Inflow = 1,
    Outflow = 2
}

public enum TaxRateType
{
    Vat = 1,
    ProfitTax = 2,
    DividendTax = 3,
    PayrollTax = 4,
    WithholdingTax = 5,
    SocialTax = 6,
    Custom = 99
}

public enum AdminExpenseStatus
{
    Draft = 1,
    Posted = 2
}

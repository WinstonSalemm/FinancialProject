namespace FinancialProject.Domain.Contexts.Sales.Enums;

public enum LeadType
{
    Deal = 1,
    Project = 2
}

public enum LeadStatus
{
    Draft = 1,
    FinanceReview = 2,
    DirectorReview = 3,
    Approved = 4,
    Rejected = 5
}

public enum ApprovalStage
{
    FinanceReview = 1,
    DirectorReview = 2
}

public enum ApprovalDecision
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public enum ProjectType
{
    SoftwareImplementation = 1,
    InstallationWork = 2,
    InfrastructureImprovement = 3,
    FinancialAudit = 4,
    AccountingAudit = 5,
    Custom = 99
}

public enum DealStatus
{
    PendingExecution = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4
}

public enum ProjectStatus
{
    PendingExecution = 1,
    InProgress = 2,
    Completed = 3,
    Cancelled = 4
}

public enum CostLineCategory
{
    Labor = 1,
    Logistics = 2,
    Supplies = 3,
    Transport = 4,
    Additional = 5,
    Other = 6
}

public enum ObligationStatus
{
    Open = 1,
    InProgress = 2,
    Fulfilled = 3,
    Overdue = 4,
    Cancelled = 5
}

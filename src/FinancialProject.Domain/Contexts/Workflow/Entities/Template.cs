using FinancialProject.Domain.Common;
using FinancialProject.Domain.Contexts.Workflow.Enums;

namespace FinancialProject.Domain.Contexts.Workflow.Entities;

public class Template : TenantSoftDeleteEntity<Guid>, IFinancialAggregateRoot
{
    public string Name { get; private set; } = null!;
    public TemplateType Type { get; private set; }
    public string? Description { get; private set; }
    public string StructureJson { get; private set; } = "{}";
    public decimal AverageMarginPercent { get; private set; }
    public decimal AverageProfitabilityPercent { get; private set; }
    public int AverageDurationDays { get; private set; }
    public string? TypicalRisksJson { get; private set; }
    public string? TypicalExpensesJson { get; private set; }
    public Guid? SourceDealId { get; private set; }
    public Guid? SourceProjectId { get; private set; }

    private Template()
    {
    }

    public static Template Create(
        Guid organizationId,
        string name,
        TemplateType type,
        string structureJson,
        decimal averageMarginPercent,
        decimal averageProfitabilityPercent,
        int averageDurationDays,
        Guid createdBy,
        string? description = null,
        string? typicalRisksJson = null,
        string? typicalExpensesJson = null,
        Guid? sourceDealId = null,
        Guid? sourceProjectId = null)
    {
        var entity = new Template
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Template name is required.", nameof(name)) : name.Trim(),
            Type = type,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            StructureJson = string.IsNullOrWhiteSpace(structureJson) ? "{}" : structureJson,
            AverageMarginPercent = averageMarginPercent,
            AverageProfitabilityPercent = averageProfitabilityPercent,
            AverageDurationDays = averageDurationDays,
            TypicalRisksJson = string.IsNullOrWhiteSpace(typicalRisksJson) ? null : typicalRisksJson,
            TypicalExpensesJson = string.IsNullOrWhiteSpace(typicalExpensesJson) ? null : typicalExpensesJson,
            SourceDealId = sourceDealId,
            SourceProjectId = sourceProjectId
        };

        entity.SetIdentity(Guid.NewGuid());
        entity.SetOrganization(organizationId);
        entity.MarkCreated(createdBy, DateTimeOffset.UtcNow);
        return entity;
    }
}

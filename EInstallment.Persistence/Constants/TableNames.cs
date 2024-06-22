namespace EInstallment.Persistence.Constants;

internal static class TableNames
{
    internal const string CreditCard = nameof(Domain.CreditCards.CreditCard);

    internal const string Installment = nameof(Domain.Installments.Installment);

    internal const string OutboxMessage = nameof(Outbox.OutboxMessage);

    internal const string Payment = nameof(Domain.Payments.Payment);

    internal const string Member = nameof(Domain.Members.Member);
}
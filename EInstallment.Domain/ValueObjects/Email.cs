﻿using EInstallment.Domain.Errors;
using EInstallment.Domain.SeedWork;
using EInstallment.Domain.Shared;

namespace EInstallment.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public const int MaxLength = 255;

    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string email) =>
        Result.Create(email)
                .Ensure(e =>
                    !string.IsNullOrWhiteSpace(e),
                    DomainErrors.Email.Empty)
                .Ensure(e =>
                    e.Length <= MaxLength,
                    DomainErrors.Email.OverSize)
                .Ensure(e =>
                    e.Split("@").Length == 2,
                    DomainErrors.Email.InvalidFormat)
                .Map(e =>
                    new Email(e));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
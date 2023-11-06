using EInstallment.Domain.Shared;

namespace EInstallment.Domain.Errors;

internal static class DomainErrors
{
    public static class Email
    {
        public static readonly Error Empty = new(
            "EInstallment.EmailCreate",
            "The email can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.EmailCreate",
            $"The email can't over {ValueObjects.Email.MaxLength}");

        public static readonly Error InvalidFormat = new(
            "EInstallment.EmailCreate",
            "The email is not email pattern");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "EInstallment.FirstNameCreate",
            "The first name can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.FirstNameCreate",
            $"The first name can't over {ValueObjects.FirstName.MaxLength}");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "EInstallment.LastNameCreate",
            "The last name can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.LastNameCreate",
            $"The last name can't over {ValueObjects.LastName.MaxLength}");
    }

    public static class BankName
    {
        public static readonly Error Empty = new(
            "EInstallment.BankNameCreate",
            "The bank name can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.BankNameCreate",
            $"The bank name can't over {ValueObjects.BankName.MaxLength}");
    }
}
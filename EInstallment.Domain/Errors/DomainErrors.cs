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

    public static class CreditCardName
    {
        public static readonly Error Empty = new(
            "EInstallment.BankNameCreate",
            "The bank name can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.BankNameCreate",
            $"The bank name can't over {ValueObjects.CreditCardName.MaxLength}");
    }

    public static class Member
    {
        public static readonly Error EmailIsNotUnique = new(
                "EInstallment.EmailIsNotUnique",
                "Email was used, please use other email address");

        public static readonly Error NotExist = new(
            "EInstallment.MemberUpdate",
            "Member is not exist");
    }

    public static class CreditCard
    {
        public static readonly Error CreditCardNameIsNotUnique = new(
            "EInstallment.CreditCardCreate",
            "Credit card name was used, please use other name");
    }
}
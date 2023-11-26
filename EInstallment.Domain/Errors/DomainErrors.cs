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
            "EInstallment.CreditCardNameCreate",
            "The credit card name can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.CreditCardNameCreate",
            $"The credit card name can't over {ValueObjects.CreditCardName.MaxLength}");
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

    public static class ItemName
    {
        public static readonly Error Empty = new(
            "EInstallment.ItemNameCreate",
            "The item name can't be empty");

        public static readonly Error OverSize = new(
            "EInstallment.ItemNameCreate",
            $"The item name can't over {ValueObjects.ItemName.MaxLength}");
    }

    public static class Installment
    {
        public static readonly Error TotalNumberOfInstallmentLessThanOne = new(
                "EInstallment.CreateInstallment",
                "The total number of installment can't less than one month");

        public static readonly Error TotalAmountLessThanOne = new(
                "EInstallment.CreateInstallment",
                "The total amount can't less than $1 NTD");

        public static readonly Error AmountOfEachInstallmentLessThanOne = new(
                "EInstallment.CreateInstallment",
                "The amount of each installment can't less than $1 NTD");

        public static readonly Error StatusIsNotUpcoming = new(
                "EInstallment.UpdateInstallment",
                "The installment status isn't upcoming");
    }

    public static class Payment
    {
        public static readonly Error AmountLessThanOne = new(
                "EInstallment.CreatePayment",
                "The amount can't less than $1 NTD");
    }
}
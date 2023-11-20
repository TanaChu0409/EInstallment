using EInstallment.Domain.Members;
using EInstallment.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EInstallment.Persistence.Configurations;

internal class MemberEntityTypeConfiguration
    : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> memberBuilder)
    {
        memberBuilder.ToTable(nameof(Member));
        memberBuilder.HasKey(x => x.Id);

        memberBuilder.OwnsOne(x => x.FirstName)
                     .Property(x => x.Value)
                     .HasColumnName(nameof(FirstName))
                     .IsUnicode(true)
                     .HasMaxLength(FirstName.MaxLength);
        memberBuilder.OwnsOne(x => x.LastName)
                     .Property(x => x.Value)
                     .HasColumnName(nameof(LastName))
                     .IsUnicode(true)
                     .HasMaxLength(LastName.MaxLength);
        memberBuilder.OwnsOne(x => x.Email)
                     .Property(x => x.Value)
                     .HasColumnName(nameof(Email))
                     .IsUnicode(true)
                     .HasMaxLength(Email.MaxLength);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.EntityTypeConfigurations;

public class OrderEntityTypeConfigurations : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserName).IsRequired();
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();
        builder.Property(x => x.EmailAddress).IsRequired();

        var addressBuilder = builder.OwnsOne(x => x.Address);
        addressBuilder.Property(x=>x.Country).IsRequired();

        var paymentDetailBuilder=builder.OwnsOne(x=>x.PaymentDetails);
        paymentDetailBuilder.Property(x=>x.PaymentMethod).IsRequired();
        paymentDetailBuilder.Property(x=>x.Cvv).IsRequired();
        paymentDetailBuilder.Property(x=>x.CardNumber).IsRequired();
    }
}

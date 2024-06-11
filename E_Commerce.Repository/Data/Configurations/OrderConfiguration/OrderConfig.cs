using E_Commerce.Core.Entities.Order_Module;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repository.Data.Configurations.OrderConfiguration
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
           .HasConversion(OSDB=>OSDB.ToString(),OSAPP=>(OrderStatus)Enum.Parse(typeof(OrderStatus), OSAPP));

            builder.Property(o=>o.SubTotal).HasColumnType("decimal(18,2)");

            builder.OwnsOne(o => o.ShippingAddress, SA=>SA.WithOwner());

            builder.HasOne(o=>o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using BookShopCore;

namespace BookShopEntityFramework
{
  class BookConfiguration : IEntityTypeConfiguration<Book>
  {
    public void Configure(EntityTypeBuilder<Book> builder)
    {
      builder.HasKey(p => p.Id);
      builder.Property(p => p.Id).ValueGeneratedOnAdd();

      builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
      builder.Property(p => p.Genre).HasMaxLength(256).IsRequired();
      builder.Property(p => p.ReceiptDate).IsRequired();
      builder.Property(p => p.Cost).IsRequired();
    }
  }
}
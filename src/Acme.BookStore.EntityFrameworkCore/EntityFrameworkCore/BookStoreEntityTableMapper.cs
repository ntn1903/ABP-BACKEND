using Acme.BookStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Acme.BookStore.EntityFrameworkCore
{
    public static class BookStoreEntityTableMapper
    {
        public static void ToBookStoreTable<TEntity>(this EntityTypeBuilder<TEntity> entity, string tableName) where TEntity : class
        {
            entity.ToTable($"{BookStoreConsts.DbTablePrefix}{tableName}", BookStoreConsts.DbSchema);
        }

        public static void ConfigureBookStoreTable(this ModelBuilder modelBuilder)
        {
            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(BookStoreConsts.DbTablePrefix + "YourEntities", BookStoreConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});

            // Book
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToBookStoreTable("Books");
                entity.ConfigureByConvention(); //auto configure for the base class props
            });

            // Configuration
            modelBuilder.Entity<Configuration>(entity =>
            {
                entity.ToBookStoreTable("Configurations");
                entity.ConfigureByConvention(); //auto configure for the base class props
            });
        }
    }
}

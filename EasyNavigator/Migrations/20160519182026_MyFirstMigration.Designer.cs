using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using EasyNavigator.Libs;

namespace EasyNavigator.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20160519182026_MyFirstMigration")]
    partial class MyFirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("EasyNavigator.Schemas.AddressSchema", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Address");

                    b.Property<string>("Lat");

                    b.Property<string>("Lng");

                    b.Property<string>("Name");

                    b.Property<string>("Type");

                    b.HasKey("Id");
                });
        }
    }
}

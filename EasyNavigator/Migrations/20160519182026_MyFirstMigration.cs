using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace EasyNavigator.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddressSchema",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Lat = table.Column<string>(nullable: true),
                    Lng = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressSchema", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("AddressSchema");
        }
    }
}

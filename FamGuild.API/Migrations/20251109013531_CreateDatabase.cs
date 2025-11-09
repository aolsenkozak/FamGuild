using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamGuild.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecurringItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Amount_Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount_CurrencyCode = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Recurrence_StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Recurrence_EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Recurrence_Frequency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringItems", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringItems");
        }
    }
}

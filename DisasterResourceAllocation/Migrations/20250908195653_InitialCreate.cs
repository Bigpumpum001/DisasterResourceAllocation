using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisasterResourceAllocation.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "areas",
                columns: table => new
                {
                    area_id = table.Column<string>(type: "text", nullable: false),
                    urgency_level = table.Column<int>(type: "integer", nullable: false),
                    required_resources = table.Column<Dictionary<string, int>>(type: "jsonb", nullable: false),
                    time_constraint = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_areas", x => x.area_id);
                });

            migrationBuilder.CreateTable(
                name: "trucks",
                columns: table => new
                {
                    truck_id = table.Column<string>(type: "text", nullable: false),
                    available_resources = table.Column<Dictionary<string, int>>(type: "jsonb", nullable: false),
                    travel_time_to_area = table.Column<Dictionary<string, int>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trucks", x => x.truck_id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "areas");

            migrationBuilder.DropTable(
                name: "trucks");
        }
    }
}

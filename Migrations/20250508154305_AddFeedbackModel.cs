using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiplomaProject_ITMO.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaterialCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Width = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FoundationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WindowCount = table.Column<int>(type: "int", nullable: false),
                    HasSauna = table.Column<bool>(type: "bit", nullable: false),
                    HasFence = table.Column<bool>(type: "bit", nullable: false),
                    HasElectricity = table.Column<bool>(type: "bit", nullable: false),
                    HasWater = table.Column<bool>(type: "bit", nullable: false),
                    NeedsWorkers = table.Column<bool>(type: "bit", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}

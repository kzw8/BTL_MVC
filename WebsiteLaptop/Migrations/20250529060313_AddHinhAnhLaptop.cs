using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteLaptop.Migrations
{
    /// <inheritdoc />
    public partial class AddHinhAnhLaptop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HinhAnhLaptop",
                columns: table => new
                {
                    MaAnh = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DuongDan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaLaptop = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HinhAnhLaptop", x => x.MaAnh);
                    table.ForeignKey(
                        name: "FK_HinhAnhLaptop_Laptop_MaLaptop",
                        column: x => x.MaLaptop,
                        principalTable: "Laptop",
                        principalColumn: "MaLaptop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HinhAnhLaptop_MaLaptop",
                table: "HinhAnhLaptop",
                column: "MaLaptop");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HinhAnhLaptop");
        }
    }
}

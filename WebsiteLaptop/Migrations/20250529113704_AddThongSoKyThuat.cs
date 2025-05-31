using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteLaptop.Migrations
{
    /// <inheritdoc />
    public partial class AddThongSoKyThuat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaThuongHieu",
                table: "Laptop");

            migrationBuilder.CreateTable(
                name: "ThongSoKyThuat",
                columns: table => new
                {
                    MaThongSo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaLaptop = table.Column<int>(type: "int", nullable: false),
                    CPU = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RAM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OCung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CardDoHoa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManHinh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CongGiaoTiep = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LAN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WIFI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bluetooth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Webcam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeDieuHanh = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrongLuong = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongSoKyThuat", x => x.MaThongSo);
                    table.ForeignKey(
                        name: "FK_ThongSoKyThuat_Laptop_MaLaptop",
                        column: x => x.MaLaptop,
                        principalTable: "Laptop",
                        principalColumn: "MaLaptop",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongSoKyThuat_MaLaptop",
                table: "ThongSoKyThuat",
                column: "MaLaptop",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongSoKyThuat");

            migrationBuilder.AddColumn<int>(
                name: "MaThuongHieu",
                table: "Laptop",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

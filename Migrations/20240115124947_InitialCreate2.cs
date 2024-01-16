using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proiect.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Hotel_HotelID",
                table: "Room");

            migrationBuilder.AlterColumn<int>(
                name: "HotelID",
                table: "Room",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Hotel_HotelID",
                table: "Room",
                column: "HotelID",
                principalTable: "Hotel",
                principalColumn: "HotelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Room_Hotel_HotelID",
                table: "Room");

            migrationBuilder.AlterColumn<int>(
                name: "HotelID",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_Hotel_HotelID",
                table: "Room",
                column: "HotelID",
                principalTable: "Hotel",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

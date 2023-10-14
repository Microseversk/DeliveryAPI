using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryApi.Migrations
{
    /// <inheritdoc />
    public partial class update_dish : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Dish");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Dish",
                newName: "DishCategory");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dish",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Dish",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Dish",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Dish");

            migrationBuilder.RenameColumn(
                name: "DishCategory",
                table: "Dish",
                newName: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Dish",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Dish",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryApi.Migrations
{
    /// <inheritdoc />
    public partial class user_dto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Dish__DishingCart_DishingCartUserEmail",
                table: "_Dish");

            migrationBuilder.DropForeignKey(
                name: "FK__Rating__Users_UserEmail",
                table: "_Rating");

            migrationBuilder.DropTable(
                name: "_Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Rating",
                table: "_Rating");

            migrationBuilder.DropIndex(
                name: "IX__Rating_UserEmail",
                table: "_Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK__DishingCart",
                table: "_DishingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Dish",
                table: "_Dish");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "_Rating");

            migrationBuilder.RenameTable(
                name: "_Rating",
                newName: "Rating");

            migrationBuilder.RenameTable(
                name: "_DishingCart",
                newName: "DishingCart");

            migrationBuilder.RenameTable(
                name: "_Dish",
                newName: "Dish");

            migrationBuilder.RenameIndex(
                name: "IX__Dish_DishingCartUserEmail",
                table: "Dish",
                newName: "IX_Dish_DishingCartUserEmail");

            migrationBuilder.AddColumn<Guid>(
                name: "UserDtoId",
                table: "Rating",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "DishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishingCart",
                table: "DishingCart",
                column: "UserEmail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dish",
                table: "Dish",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_UserDtoId",
                table: "Rating",
                column: "UserDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_DishingCart_DishingCartUserEmail",
                table: "Dish",
                column: "DishingCartUserEmail",
                principalTable: "DishingCart",
                principalColumn: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Users_UserDtoId",
                table: "Rating",
                column: "UserDtoId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_DishingCart_DishingCartUserEmail",
                table: "Dish");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Users_UserDtoId",
                table: "Rating");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_UserDtoId",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishingCart",
                table: "DishingCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dish",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "UserDtoId",
                table: "Rating");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "_Rating");

            migrationBuilder.RenameTable(
                name: "DishingCart",
                newName: "_DishingCart");

            migrationBuilder.RenameTable(
                name: "Dish",
                newName: "_Dish");

            migrationBuilder.RenameIndex(
                name: "IX_Dish_DishingCartUserEmail",
                table: "_Dish",
                newName: "IX__Dish_DishingCartUserEmail");

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "_Rating",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Rating",
                table: "_Rating",
                column: "DishId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__DishingCart",
                table: "_DishingCart",
                column: "UserEmail");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Dish",
                table: "_Dish",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "_Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users", x => x.Email);
                });

            migrationBuilder.CreateIndex(
                name: "IX__Rating_UserEmail",
                table: "_Rating",
                column: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK__Dish__DishingCart_DishingCartUserEmail",
                table: "_Dish",
                column: "DishingCartUserEmail",
                principalTable: "_DishingCart",
                principalColumn: "UserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK__Rating__Users_UserEmail",
                table: "_Rating",
                column: "UserEmail",
                principalTable: "_Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

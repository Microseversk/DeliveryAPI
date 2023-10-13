using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryApi.Migrations
{
    /// <inheritdoc />
    public partial class rtrtr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_DishingCart",
                columns: table => new
                {
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DishingCart", x => x.UserEmail);
                });

            migrationBuilder.CreateTable(
                name: "_Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "_Dish",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVegetarian = table.Column<bool>(type: "bit", nullable: false),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DishingCartUserEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Dish", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Dish__DishingCart_DishingCartUserEmail",
                        column: x => x.DishingCartUserEmail,
                        principalTable: "_DishingCart",
                        principalColumn: "UserEmail");
                });

            migrationBuilder.CreateTable(
                name: "_Rating",
                columns: table => new
                {
                    DishId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Rating", x => x.DishId);
                    table.ForeignKey(
                        name: "FK__Rating__Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "_Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX__Dish_DishingCartUserEmail",
                table: "_Dish",
                column: "DishingCartUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX__Rating_UserEmail",
                table: "_Rating",
                column: "UserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_Dish");

            migrationBuilder.DropTable(
                name: "_Rating");

            migrationBuilder.DropTable(
                name: "_DishingCart");

            migrationBuilder.DropTable(
                name: "_Users");
        }
    }
}

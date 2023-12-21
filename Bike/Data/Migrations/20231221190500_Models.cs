using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bike.Data.Migrations
{
    /// <inheritdoc />
    public partial class Models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DoljnostId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Midname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BikeType",
                columns: table => new
                {
                    BikeTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Complexity = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BikeType", x => x.BikeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Doljnosts",
                columns: table => new
                {
                    DoljnostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoljnostName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doljnosts", x => x.DoljnostId);
                });

            migrationBuilder.CreateTable(
                name: "Height",
                columns: table => new
                {
                    HeightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Terrain_height = table.Column<int>(type: "int", nullable: false),
                    Complexity = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Height", x => x.HeightId);
                });

            migrationBuilder.CreateTable(
                name: "Route_CountOtchet",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: true),
                    nm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    kol = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "MainAddress",
                columns: table => new
                {
                    MainAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    House = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeightId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainAddress", x => x.MainAddressId);
                    table.ForeignKey(
                        name: "FK_MainAddress_Height_HeightId",
                        column: x => x.HeightId,
                        principalTable: "Height",
                        principalColumn: "HeightId");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MainAddressId = table.Column<int>(type: "int", nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    House = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_MainAddress_MainAddressId",
                        column: x => x.MainAddressId,
                        principalTable: "MainAddress",
                        principalColumn: "MainAddressId");
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressId1 = table.Column<int>(type: "int", nullable: true),
                    AddressId2 = table.Column<int>(type: "int", nullable: true),
                    BikeTypeId = table.Column<int>(type: "int", nullable: true),
                    Time = table.Column<int>(type: "int", nullable: false),
                    TimeResult = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                    table.ForeignKey(
                        name: "FK_Routes_Address_AddressId1",
                        column: x => x.AddressId1,
                        principalTable: "Address",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Routes_Address_AddressId2",
                        column: x => x.AddressId2,
                        principalTable: "Address",
                        principalColumn: "AddressId");
                    table.ForeignKey(
                        name: "FK_Routes_BikeType_BikeTypeId",
                        column: x => x.BikeTypeId,
                        principalTable: "BikeType",
                        principalColumn: "BikeTypeId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DoljnostId",
                table: "AspNetUsers",
                column: "DoljnostId");

            migrationBuilder.CreateIndex(
                name: "IX_Address_MainAddressId",
                table: "Address",
                column: "MainAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_MainAddress_HeightId",
                table: "MainAddress",
                column: "HeightId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AddressId1",
                table: "Routes",
                column: "AddressId1");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_AddressId2",
                table: "Routes",
                column: "AddressId2");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_BikeTypeId",
                table: "Routes",
                column: "BikeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Doljnosts_DoljnostId",
                table: "AspNetUsers",
                column: "DoljnostId",
                principalTable: "Doljnosts",
                principalColumn: "DoljnostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Doljnosts_DoljnostId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Doljnosts");

            migrationBuilder.DropTable(
                name: "Route_CountOtchet");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "BikeType");

            migrationBuilder.DropTable(
                name: "MainAddress");

            migrationBuilder.DropTable(
                name: "Height");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DoljnostId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DoljnostId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Midname",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "AspNetUsers");
        }
    }
}

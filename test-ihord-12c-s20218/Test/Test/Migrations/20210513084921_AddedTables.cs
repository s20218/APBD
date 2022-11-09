using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Test.Migrations
{
    public partial class AddedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityDict",
                columns: table => new
                {
                    IdCityDict = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CityDict_pk", x => x.IdCityDict);
                });

            migrationBuilder.CreateTable(
                name: "Passenger",
                columns: table => new
                {
                    IdPassenger = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    PassportNum = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Passenger_pk", x => x.IdPassenger);
                });

            migrationBuilder.CreateTable(
                name: "Plane",
                columns: table => new
                {
                    IdPlane = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxSeats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Plane_pk", x => x.IdPlane);
                });

            migrationBuilder.CreateTable(
                name: "Flight",
                columns: table => new
                {
                    IdFlight = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IdPlane = table.Column<int>(type: "int", nullable: false),
                    IdCItyDict = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Flight_pk", x => x.IdFlight);
                    table.ForeignKey(
                        name: "Flight_CityDict",
                        column: x => x.IdCItyDict,
                        principalTable: "CityDict",
                        principalColumn: "IdCityDict",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Flight_Plane",
                        column: x => x.IdPlane,
                        principalTable: "Plane",
                        principalColumn: "IdPlane",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Flight_Passenger",
                columns: table => new
                {
                    IdFlight = table.Column<int>(type: "int", nullable: false),
                    IdPassenger = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Flight_Passenger_pk", x => new { x.IdFlight, x.IdPassenger });
                    table.ForeignKey(
                        name: "Flight_Passenger_Flight",
                        column: x => x.IdFlight,
                        principalTable: "Flight",
                        principalColumn: "IdFlight",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "Flight_Passenger_Passenger",
                        column: x => x.IdPassenger,
                        principalTable: "Passenger",
                        principalColumn: "IdPassenger",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CityDict",
                columns: new[] { "IdCityDict", "City" },
                values: new object[,]
                {
                    { 1, "Warsaw" },
                    { 2, "Berlin" }
                });

            migrationBuilder.InsertData(
                table: "Passenger",
                columns: new[] { "IdPassenger", "FirstName", "LastName", "PassportNum" },
                values: new object[,]
                {
                    { 1, "Jan", "Kowalski", "1234567890" },
                    { 2, "Tom", "Reddle", "551662883" }
                });

            migrationBuilder.InsertData(
                table: "Plane",
                columns: new[] { "IdPlane", "MaxSeats", "Name" },
                values: new object[,]
                {
                    { 1, 150, "Jet321" },
                    { 2, 250, "Boeng456" }
                });

            migrationBuilder.InsertData(
                table: "Flight",
                columns: new[] { "IdFlight", "Comment", "FlightDate", "IdCItyDict", "IdPlane" },
                values: new object[] { 1, "default", new DateTime(2021, 5, 13, 10, 49, 20, 654, DateTimeKind.Local).AddTicks(6515), 2, 1 });

            migrationBuilder.InsertData(
                table: "Flight_Passenger",
                columns: new[] { "IdFlight", "IdPassenger" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Flight_Passenger",
                columns: new[] { "IdFlight", "IdPassenger" },
                values: new object[] { 1, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Flight_IdCItyDict",
                table: "Flight",
                column: "IdCItyDict");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_IdPlane",
                table: "Flight",
                column: "IdPlane");

            migrationBuilder.CreateIndex(
                name: "IX_Flight_Passenger_IdPassenger",
                table: "Flight_Passenger",
                column: "IdPassenger");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flight_Passenger");

            migrationBuilder.DropTable(
                name: "Flight");

            migrationBuilder.DropTable(
                name: "Passenger");

            migrationBuilder.DropTable(
                name: "CityDict");

            migrationBuilder.DropTable(
                name: "Plane");
        }
    }
}

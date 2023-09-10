using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Worker_GrpcService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Genders",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Workers",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    lastName = table.Column<string>(type: "text", nullable: false),
                    patronymic = table.Column<string>(type: "text", nullable: false),
                    birthDate = table.Column<string>(type: "text", nullable: false),
                    hasChildren = table.Column<bool>(type: "boolean", nullable: false),
                    GenderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workers", x => x.id);
                    table.ForeignKey(
                        name: "FK_Workers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "public",
                        principalTable: "Genders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Genders",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Женский" },
                    { 2, "Мужской" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Workers",
                columns: new[] { "id", "birthDate", "name", "GenderId", "hasChildren", "lastName", "patronymic" },
                values: new object[] { 1, "14.11.2001", "Валерия", 1, false, "Гордеева", "Александровна" });

            migrationBuilder.CreateIndex(
                name: "IX_Workers_GenderId",
                schema: "public",
                table: "Workers",
                column: "GenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Workers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "public");
        }
    }
}

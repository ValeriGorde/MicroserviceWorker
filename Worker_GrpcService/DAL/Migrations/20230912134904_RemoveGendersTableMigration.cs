using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Worker_GrpcService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGendersTableMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Workers_Genders_GenderId",
                schema: "public",
                table: "Workers");

            migrationBuilder.DropTable(
                name: "Genders",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_Workers_GenderId",
                schema: "public",
                table: "Workers");

            migrationBuilder.DropColumn(
                name: "GenderId",
                schema: "public",
                table: "Workers");

            migrationBuilder.AddColumn<string>(
                name: "gender",
                schema: "public",
                table: "Workers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Workers",
                keyColumn: "id",
                keyValue: 1,
                column: "gender",
                value: "Женский");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                schema: "public",
                table: "Workers");

            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                schema: "public",
                table: "Workers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.InsertData(
                schema: "public",
                table: "Genders",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Женский" },
                    { 2, "Мужской" }
                });

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Workers",
                keyColumn: "id",
                keyValue: 1,
                column: "GenderId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Workers_GenderId",
                schema: "public",
                table: "Workers",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Workers_Genders_GenderId",
                schema: "public",
                table: "Workers",
                column: "GenderId",
                principalSchema: "public",
                principalTable: "Genders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

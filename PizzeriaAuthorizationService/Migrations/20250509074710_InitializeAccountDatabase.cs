using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountDatabaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitializeAccountDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "cached_passsword",
                table: "user",
                newName: "hashed_passsword");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hashed_passsword",
                table: "user",
                newName: "cached_passsword");
        }
    }
}

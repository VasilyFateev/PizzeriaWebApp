using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PizzeriaAuthorizationService.Migrations
{
    /// <inheritdoc />
    public partial class CreateAccountDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "adress",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    region = table.Column<string>(type: "text", nullable: false),
                    settlement = table.Column<string>(type: "text", nullable: false),
                    street_name = table.Column<string>(type: "text", nullable: false),
                    building_number = table.Column<int>(type: "integer", nullable: false),
                    liter = table.Column<string>(type: "text", nullable: true),
                    apartment_number = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_adress", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    cached_passsword = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shopping_cart",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_cart", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_cart_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_adress",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    adress_id = table.Column<long>(type: "bigint", nullable: false),
                    id_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_adress", x => new { x.user_id, x.adress_id });
                    table.ForeignKey(
                        name: "FK_adress_user_id",
                        column: x => x.adress_id,
                        principalTable: "adress",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_adress_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_payment_method",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    payment_agregator_user_id = table.Column<string>(type: "text", nullable: false),
                    bank_card_last_numbers = table.Column<string>(type: "text", nullable: false),
                    id_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_payment_method", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_payment_method_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_cart_item",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cart_id = table.Column<long>(type: "bigint", nullable: false),
                    product_item_id = table.Column<long>(type: "bigint", nullable: false),
                    count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_cart_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_cart_item_shopping_cart_id",
                        column: x => x.cart_id,
                        principalTable: "shopping_cart",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shopping_cart_user_id",
                table: "shopping_cart",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_cart_item_cart_id",
                table: "shopping_cart_item",
                column: "cart_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_adress_adress_id",
                table: "user_adress",
                column: "adress_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_payment_method_user_id",
                table: "user_payment_method",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shopping_cart_item");

            migrationBuilder.DropTable(
                name: "user_adress");

            migrationBuilder.DropTable(
                name: "user_payment_method");

            migrationBuilder.DropTable(
                name: "shopping_cart");

            migrationBuilder.DropTable(
                name: "adress");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}

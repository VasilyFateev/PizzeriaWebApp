using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DatabaseAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_category_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    image_link = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_product_category_id",
                        column: x => x.product_category_id,
                        principalTable: "product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "variation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_category_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variation", x => x.id);
                    table.ForeignKey(
                        name: "FK_variation_product_category_id",
                        column: x => x.product_category_id,
                        principalTable: "product_category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_item",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    cooking_time = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_item_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "variation_option",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    variation_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variation_option", x => x.id);
                    table.ForeignKey(
                        name: "FK_variation_option_variation_id",
                        column: x => x.variation_id,
                        principalTable: "variation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_configuration",
                columns: table => new
                {
                    product_item_id = table.Column<int>(type: "integer", nullable: false),
                    variation_option_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_configuration", x => new { x.product_item_id, x.variation_option_id });
                    table.ForeignKey(
                        name: "FK_product_configuration_product_item_id",
                        column: x => x.product_item_id,
                        principalTable: "product_item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_configuration_variation_option_id",
                        column: x => x.variation_option_id,
                        principalTable: "variation_option",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_product_category_id",
                table: "product",
                column: "product_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_configuration_variation_option_id",
                table: "product_configuration",
                column: "variation_option_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_item_product_id",
                table: "product_item",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_variation_product_category_id",
                table: "variation",
                column: "product_category_id");

            migrationBuilder.CreateIndex(
                name: "IX_variation_option_variation_id",
                table: "variation_option",
                column: "variation_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_configuration");

            migrationBuilder.DropTable(
                name: "product_item");

            migrationBuilder.DropTable(
                name: "variation_option");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "variation");

            migrationBuilder.DropTable(
                name: "product_category");
        }
    }
}

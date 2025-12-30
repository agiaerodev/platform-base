using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Platform.Migrations
{
    /// <inheritdoc />
    public partial class IcommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "icomments");

            migrationBuilder.CreateTable(
                name: "Icomments",
                schema: "icomments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    comment = table.Column<string>(type: "text", nullable: true),
                    approved = table.Column<bool>(type: "bit", nullable: true),
                    is_internal = table.Column<bool>(type: "bit", nullable: true),
                    commentable_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    commentable_id = table.Column<long>(type: "bigint", nullable: true),
                    guest_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    guest_email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    restored_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<long>(type: "bigint", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: true),
                    deleted_by = table.Column<long>(type: "bigint", nullable: true),
                    restored_by = table.Column<long>(type: "bigint", nullable: true),
                    external_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    offline_id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    options = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Icomments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Icomments_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "iprofile",
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Icomments_user_id",
                schema: "icomments",
                table: "Icomments",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Icomments",
                schema: "icomments");
        }
    }
}

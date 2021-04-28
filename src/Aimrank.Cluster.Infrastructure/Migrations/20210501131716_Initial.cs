using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aimrank.Cluster.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cluster");

            migrationBuilder.CreateTable(
                name: "inbox_messages",
                schema: "cluster",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    data = table.Column<string>(type: "text", nullable: true),
                    occurred_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    processed_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_messages",
                schema: "cluster",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    data = table.Column<string>(type: "text", nullable: true),
                    occurred_on = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    processed_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pods",
                schema: "cluster",
                columns: table => new
                {
                    ip_address = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    max_servers = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pods", x => x.ip_address);
                });

            migrationBuilder.CreateTable(
                name: "steam_tokens",
                schema: "cluster",
                columns: table => new
                {
                    token = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_steam_tokens", x => x.token);
                });

            migrationBuilder.CreateTable(
                name: "servers",
                schema: "cluster",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_accepted = table.Column<bool>(type: "boolean", nullable: false),
                    steam_token_token = table.Column<string>(type: "character varying(32)", nullable: false),
                    pod_ip_address = table.Column<string>(type: "character varying(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_servers", x => x.id);
                    table.ForeignKey(
                        name: "fk_servers_pods_pod_ip_address",
                        column: x => x.pod_ip_address,
                        principalSchema: "cluster",
                        principalTable: "pods",
                        principalColumn: "ip_address",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_servers_steam_tokens_steam_token_token",
                        column: x => x.steam_token_token,
                        principalSchema: "cluster",
                        principalTable: "steam_tokens",
                        principalColumn: "token",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_servers_pod_ip_address",
                schema: "cluster",
                table: "servers",
                column: "pod_ip_address");

            migrationBuilder.CreateIndex(
                name: "ix_servers_steam_token_token",
                schema: "cluster",
                table: "servers",
                column: "steam_token_token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_messages",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "outbox_messages",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "servers",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "pods",
                schema: "cluster");

            migrationBuilder.DropTable(
                name: "steam_tokens",
                schema: "cluster");
        }
    }
}

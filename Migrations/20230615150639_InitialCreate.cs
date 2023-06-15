using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dievas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    CampaignId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.CampaignId);
                });

            migrationBuilder.CreateTable(
                name: "MessageTypes",
                columns: table => new
                {
                    MessageTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageTypes", x => x.MessageTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SettingTypes",
                columns: table => new
                {
                    SettingTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    InputControlType = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingTypes", x => x.SettingTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Roles = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    MessageTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Approved = table.Column<bool>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Emergent = table.Column<bool>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_MessageTypes_MessageTypeId",
                        column: x => x.MessageTypeId,
                        principalTable: "MessageTypes",
                        principalColumn: "MessageTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    SystemSettingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Field = table.Column<string>(type: "TEXT", nullable: true),
                    Value = table.Column<string>(type: "TEXT", nullable: true),
                    SettingTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.SystemSettingId);
                    table.ForeignKey(
                        name: "FK_SystemSettings_SettingTypes_SettingTypeId",
                        column: x => x.SettingTypeId,
                        principalTable: "SettingTypes",
                        principalColumn: "SettingTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMessage",
                columns: table => new
                {
                    CampaignsCampaignId = table.Column<int>(type: "INTEGER", nullable: false),
                    MessagesMessageId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMessage", x => new { x.CampaignsCampaignId, x.MessagesMessageId });
                    table.ForeignKey(
                        name: "FK_CampaignMessage_Campaigns_CampaignsCampaignId",
                        column: x => x.CampaignsCampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "CampaignId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignMessage_Messages_MessagesMessageId",
                        column: x => x.MessagesMessageId,
                        principalTable: "Messages",
                        principalColumn: "MessageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MessageTypes",
                columns: new[] { "MessageTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "full-page" },
                    { 2, "marquee" }
                });

            migrationBuilder.InsertData(
                table: "SettingTypes",
                columns: new[] { "SettingTypeId", "InputControlType", "Name" },
                values: new object[,]
                {
                    { 1, "text", "text" },
                    { 2, "button", "button" },
                    { 3, "checkbox", "checkbox" },
                    { 4, "color", "color" },
                    { 5, "date", "date" },
                    { 6, "datetime-local", "datetime-local" },
                    { 7, "email", "email" },
                    { 8, "file", "file" },
                    { 9, "hidden", "hidden" },
                    { 10, "image", "image" },
                    { 11, "month", "month" },
                    { 12, "number", "number" },
                    { 13, "password", "password" },
                    { 14, "radio", "radio" },
                    { 15, "range", "range" },
                    { 16, "reset", "reset" },
                    { 17, "search", "search" },
                    { 18, "submit", "submit" },
                    { 19, "tel", "tel" },
                    { 20, "time", "time" },
                    { 21, "url", "url" },
                    { 22, "week", "week" },
                    { 23, "int", "int" },
                    { 24, "float", "float" },
                    { 25, "double", "double" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMessage_MessagesMessageId",
                table: "CampaignMessage",
                column: "MessagesMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageTypeId",
                table: "Messages",
                column: "MessageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_SettingTypeId",
                table: "SystemSettings",
                column: "SettingTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignMessage");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SettingTypes");

            migrationBuilder.DropTable(
                name: "MessageTypes");
        }
    }
}

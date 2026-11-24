using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TPass_Data.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedDemoData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "DisplayName", "Email", "EmailConfirmed", "IsDeleted", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "tpass-demo-user-1", 0, "tpass-demo-concurrency-1", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Demo User 1", "demo1@tpass.example", true, false, false, null, "DEMO1@TPASS.EXAMPLE", "DEMO1@TPASS.EXAMPLE", "AQAAAAIAAYagAAAAEAHIEnsGSrP9TsBsJTygruVclvABgP0c0ZznIUGgAk9qA2jMbv42ydGAHbvBbC2T4A==", null, false, "tpass-demo-security-1", false, "demo1@tpass.example" },
                    { "tpass-demo-user-2", 0, "tpass-demo-concurrency-2", new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Demo User 2", "demo2@tpass.example", true, false, false, null, "DEMO2@TPASS.EXAMPLE", "DEMO2@TPASS.EXAMPLE", "AQAAAAIAAYagAAAAEAHIEnsGSrP9TsBsJTygruVclvABgP0c0ZznIUGgAk9qA2jMbv42ydGAHbvBbC2T4A==", null, false, "tpass-demo-security-2", false, "demo2@tpass.example" }
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "Id", "CreatedOn", "IsDeleted", "Password", "UserId", "Username", "Website" },
                values: new object[,]
                {
                    { -2, new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, "SamplePassword2!", "tpass-demo-user-2", "demo2", "https://demo2.example" },
                    { -1, new DateTime(2026, 11, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, "SamplePassword1!", "tpass-demo-user-1", "demo1", "https://demo1.example" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "UserId" },
                values: new object[,]
                {
                    { -2, "Personal", "tpass-demo-user-2" },
                    { -1, "Work", "tpass-demo-user-1" }
                });

            migrationBuilder.InsertData(
                table: "CredentialTags",
                columns: new[] { "CredentialId", "TagId" },
                values: new object[,]
                {
                    { -2, -2 },
                    { -1, -1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CredentialTags",
                keyColumns: new[] { "CredentialId", "TagId" },
                keyValues: new object[] { -2, -2 });

            migrationBuilder.DeleteData(
                table: "CredentialTags",
                keyColumns: new[] { "CredentialId", "TagId" },
                keyValues: new object[] { -1, -1 });

            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Credentials",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "tpass-demo-user-1");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "tpass-demo-user-2");
        }
    }
}

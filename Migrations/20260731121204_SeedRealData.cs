using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GazaRealEstatePortal.Migrations
{
    /// <inheritdoc />
    public partial class SeedRealData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "ExternalProvider", "ExternalProviderId", "FullName", "IsActive", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[] { 2, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "sondosalaa687@gmail.com", null, null, "Sondos Alaa Yassin", true, "$2a$11$EbXyQBCIEPmReCYun1S4e.E7hs9PNHy1stk841TCweJ4hCK9g37mG", "0593617699", "RegisteredUser" });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Area", "Bathrooms", "CityAreaCamp", "ContactPhone", "CreatedAt", "Description", "DetailedAddress", "Features", "Floor", "Governorate", "OperationType", "Price", "PropertyType", "Rooms", "Status", "Title", "UpdatedAt", "UserId", "WhatsAppNumber" },
                values: new object[,]
                {
                    { 1, 150m, 2, "الرمال", "0593617699", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "شقة سكنية تشطيب سوبر ديلوكس في الرمال.", "شارع عمر المختار", "موقف سيارة, مصعد", "2", "غزة", "Sale", 120000m, "Apartment", 3, "Approved", "شقة سكنية فاخرة", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), 2, "00970593617699" },
                    { 2, 300m, 3, "البلد", "0593617699", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "فيلا حديثة في خانيونس البلد بمساحة ممتازة.", "وسط البلد", "حديقة, كراج", "0", "خانيونس", "Rent", 400m, "Villa", 5, "Approved", "فيلا حديثة التصميم", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), 2, "00970593617699" },
                    { 3, 1000m, 0, "بيت لاهيا", "0593617699", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "أرض زراعية خصبة في بيت لاهيا.", "منطقة العطاطرة", "بئر ماء", "0", "الشمال", "Sale", 50000m, "Land", 0, "Approved", "أرض زراعية", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), 2, "00970593617699" },
                    { 4, 85m, 1, "النصر", "0593617699", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "مكتب تجاري جاهز للعمل في النصر.", "شارع النصر", "تكييف, إنترنت", "1", "غزة", "Rent", 1200m, "Office", 2, "Approved", "مكتب تجاري مجهز", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), 2, "00970593617699" },
                    { 5, 200m, 2, "تل السلطان", "0593617699", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "منزل مستقل وواسع في تل السلطان.", "غرب تل السلطان", "حوش, تهوية ممتازة", "1", "رفح", "Sale", 85000m, "House", 4, "Approved", "منزل مستقل", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), 2, "00970593617699" },
                    { 6, 130m, 1, "النصيرات", "0593617699", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), "شقة عائلية مريحة في النصيرات.", "المخيم الجديد", "قريبة من الخدمات", "3", "الوسطى", "Sale", 60000m, "Apartment", 3, "Approved", "شقة عائلية", new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc), 2, "00970593617699" }
                });

            migrationBuilder.InsertData(
                table: "PropertyImages",
                columns: new[] { "Id", "ImageUrl", "PropertyId", "UploadedAt" },
                values: new object[,]
                {
                    { 1, "https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", 1, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc) },
                    { 2, "https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", 2, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc) },
                    { 3, "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", 3, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc) },
                    { 4, "https://images.unsplash.com/photo-1497366216548-37526070297c?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", 4, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc) },
                    { 5, "https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", 5, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc) },
                    { 6, "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80", 6, new DateTime(2026, 7, 30, 14, 55, 9, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PropertyImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PropertyImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PropertyImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PropertyImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PropertyImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PropertyImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Properties",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}

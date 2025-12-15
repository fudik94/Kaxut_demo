using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveSeedDemoQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("3f33e92e-cb63-4cc4-b0e2-238312717468"));

            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("9576aa35-31d7-4d24-8478-50ecbd9c14f5"));

            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("af714130-220e-4060-82a6-f6a913c97417"));

            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("b2585133-6c48-4a4b-931a-4452a72fa089"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("a5ab5d08-9f7f-42ea-a98d-5c29242b17e0"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("ad7f5454-be71-4fbd-8478-ba49407a30b6"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("224c892c-4463-448a-918b-6eae1aa5968f"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "Code", "CreatedAt", "Title" },
                values: new object[] { new Guid("224c892c-4463-448a-918b-6eae1aa5968f"), "QZ001", new DateTime(2025, 12, 15, 13, 53, 59, 329, DateTimeKind.Utc).AddTicks(3808), "C# Basics" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectIndex", "Order", "QuestionType", "QuizId", "Text", "TimeLimitSeconds" },
                values: new object[,]
                {
                    { new Guid("a5ab5d08-9f7f-42ea-a98d-5c29242b17e0"), 0, 2, "MultipleChoice", new Guid("224c892c-4463-448a-918b-6eae1aa5968f"), "Что такое цикл?", 15 },
                    { new Guid("ad7f5454-be71-4fbd-8478-ba49407a30b6"), 1, 1, "MultipleChoice", new Guid("224c892c-4463-448a-918b-6eae1aa5968f"), "Что такое переменная?", 15 }
                });

            migrationBuilder.InsertData(
                table: "AnswerOptions",
                columns: new[] { "Id", "ChoiceQuestionId", "IsCorrect", "MultipleChoiceQuestionId", "QuestionId", "Text" },
                values: new object[,]
                {
                    { new Guid("3f33e92e-cb63-4cc4-b0e2-238312717468"), null, false, null, new Guid("a5ab5d08-9f7f-42ea-a98d-5c29242b17e0"), "Класс в C#" },
                    { new Guid("9576aa35-31d7-4d24-8478-50ecbd9c14f5"), null, false, null, new Guid("ad7f5454-be71-4fbd-8478-ba49407a30b6"), "Тип данных" },
                    { new Guid("af714130-220e-4060-82a6-f6a913c97417"), null, true, null, new Guid("a5ab5d08-9f7f-42ea-a98d-5c29242b17e0"), "Повторяющееся выполнение блока кода" },
                    { new Guid("b2585133-6c48-4a4b-931a-4452a72fa089"), null, true, null, new Guid("ad7f5454-be71-4fbd-8478-ba49407a30b6"), "Место для хранения значения" }
                });
        }
    }
}

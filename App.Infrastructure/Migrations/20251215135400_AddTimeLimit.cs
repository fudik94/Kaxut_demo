using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("8769cd44-270e-47e7-8f90-5d22f6e8d5df"));

            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("9031c6d1-df26-4bb4-84f6-28418df8a37c"));

            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("d4578eb2-70cb-4b05-b765-6bec602d65c7"));

            migrationBuilder.DeleteData(
                table: "AnswerOptions",
                keyColumn: "Id",
                keyValue: new Guid("d83f9f95-86f2-478e-b2a4-6ee12b0da1a9"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("60d1b826-13af-41ed-95b6-195ebe4add69"));

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: new Guid("f3e06a66-0c62-4d5a-9216-c29bdcb6c878"));

            migrationBuilder.DeleteData(
                table: "Quizzes",
                keyColumn: "Id",
                keyValue: new Guid("6ff30c7f-f6e4-4e79-ba9a-edff4b430c68"));

            migrationBuilder.AddColumn<int>(
                name: "TimeLimitSeconds",
                table: "Questions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "TimeLimitSeconds",
                table: "Questions");

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "Code", "CreatedAt", "Title" },
                values: new object[] { new Guid("6ff30c7f-f6e4-4e79-ba9a-edff4b430c68"), "QZ001", new DateTime(2025, 12, 12, 9, 35, 24, 163, DateTimeKind.Utc).AddTicks(8035), "C# Basics" });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CorrectIndex", "Order", "QuestionType", "QuizId", "Text" },
                values: new object[,]
                {
                    { new Guid("60d1b826-13af-41ed-95b6-195ebe4add69"), 0, 2, "MultipleChoice", new Guid("6ff30c7f-f6e4-4e79-ba9a-edff4b430c68"), "Что такое цикл?" },
                    { new Guid("f3e06a66-0c62-4d5a-9216-c29bdcb6c878"), 1, 1, "MultipleChoice", new Guid("6ff30c7f-f6e4-4e79-ba9a-edff4b430c68"), "Что такое переменная?" }
                });

            migrationBuilder.InsertData(
                table: "AnswerOptions",
                columns: new[] { "Id", "ChoiceQuestionId", "IsCorrect", "MultipleChoiceQuestionId", "QuestionId", "Text" },
                values: new object[,]
                {
                    { new Guid("8769cd44-270e-47e7-8f90-5d22f6e8d5df"), null, true, null, new Guid("f3e06a66-0c62-4d5a-9216-c29bdcb6c878"), "Место для хранения значения" },
                    { new Guid("9031c6d1-df26-4bb4-84f6-28418df8a37c"), null, false, null, new Guid("60d1b826-13af-41ed-95b6-195ebe4add69"), "Класс в C#" },
                    { new Guid("d4578eb2-70cb-4b05-b765-6bec602d65c7"), null, false, null, new Guid("f3e06a66-0c62-4d5a-9216-c29bdcb6c878"), "Тип данных" },
                    { new Guid("d83f9f95-86f2-478e-b2a4-6ee12b0da1a9"), null, true, null, new Guid("60d1b826-13af-41ed-95b6-195ebe4add69"), "Повторяющееся выполнение блока кода" }
                });
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace App.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuestionType = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Multiple = table.Column<bool>(type: "INTEGER", nullable: true),
                    CorrectIndex = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnswerOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuestionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChoiceQuestionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MultipleChoiceQuestionId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_ChoiceQuestionId",
                        column: x => x.ChoiceQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_MultipleChoiceQuestionId",
                        column: x => x.MultipleChoiceQuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnswerOptions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionTag",
                columns: table => new
                {
                    QuestionsId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TagsId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTag", x => new { x.QuestionsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_QuestionTag_Questions_QuestionsId",
                        column: x => x.QuestionsId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_ChoiceQuestionId",
                table: "AnswerOptions",
                column: "ChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_MultipleChoiceQuestionId",
                table: "AnswerOptions",
                column: "MultipleChoiceQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnswerOptions_QuestionId",
                table: "AnswerOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionTag_TagsId",
                table: "QuestionTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_Code",
                table: "Quizzes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerOptions");

            migrationBuilder.DropTable(
                name: "QuestionTag");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}

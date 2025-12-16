using System;
using System.Linq;
using System.Threading.Tasks;
using App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Infrastructure.Data
{
    public static class DbInitializer
    {
        private const string QUIZ_1_CODE = "SYSTEM_QUIZ_1";
        private const string QUIZ_2_CODE = "SYSTEM_QUIZ_2";

        public static async Task EnsureSystemQuizzesAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            // Quiz 1: C# Quiz
            if (!context.Quizzes.Any(q => q.Code == QUIZ_1_CODE))
            {
                var quiz1 = new Quiz
                {
                    Code = QUIZ_1_CODE,
                    Title = "C# Quiz"
                };

                quiz1.Questions.Add(CreateSingleChoiceQuestion("What is the correct way to declare a variable in C#?",
                    ("var x;", false), ("int x;", true), ("x int;", false), ("x = int", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("Which of these is a reference type?",
                    ("int", false), ("string", true), ("bool", false), ("type", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("What is the entry point of a C# console application?",
                    ("Start()", false), ("Main()", true), ("Init()", false), ("End()", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("Which keyword is used for inheritance in C#?",
                    ("implements", false), (":", true), ("extends", false), ("()", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("Which of these is a value type?",
                    ("string", false), ("int", true), ("object", false), ("dict", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("How do you define a constant in C#?",
                    ("const int x = 5;", true), ("int const x = 5;", false), ("constant x = 5;", false), ("x = 5", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("Which access modifier allows access from the same assembly?",
                    ("public", false), ("private", false), ("internal", true), ("abstract", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("What does 'static' mean in C#?",
                    ("Belongs to the instance", false), ("Belongs to the class", true), ("Cannot be accessed", false), ("Nothing from the list", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("Which of these can implement multiple interfaces?",
                    ("Class", true), ("Struct", false), ("Enum", false), ("var", false)));

                quiz1.Questions.Add(CreateSingleChoiceQuestion("Which method is used to start a new thread in C#?",
                    ("Thread.Start()", true), ("Thread.Run()", false), ("Thread.Begin()", false), ("Thread.End()", false)));

                context.Quizzes.Add(quiz1);
            }

            // Quiz 2: Estonia Quiz
            if (!context.Quizzes.Any(q => q.Code == QUIZ_2_CODE))
            {
                var quiz2 = new Quiz
                {
                    Code = QUIZ_2_CODE,
                    Title = "Estonia Quiz"
                };

                quiz2.Questions.Add(CreateSingleChoiceQuestion("What is the capital of Estonia?",
                    ("Tallinn", true), ("Tartu", false), ("Narva", false), ("Rakvere", false)));

                quiz2.Questions.Add(CreateSingleChoiceQuestion("Which sea borders Estonia?",
                    ("Baltic Sea", true), ("North Sea", false), ("Black Sea", false), ("Mediterranean", false)));

                quiz2.Questions.Add(CreateSingleChoiceQuestion("What is the official language of Estonia?",
                    ("Finnish", false), ("Estonian", true), ("Russian", false), ("English", false)));

                quiz2.Questions.Add(CreateSingleChoiceQuestion("Which currency is used in Estonia?",
                    ("Euro", true), ("Dollar", false), ("Kroon", false), ("Hryvnia", false)));

                quiz2.Questions.Add(CreateSingleChoiceQuestion("What is the population of Estonia approximately?",
                    ("1.3 million", true), ("5 million", false), ("500k", false), ("8 million", false)));

                quiz2.Questions.Add(CreateSingleChoiceQuestion("Which of these is a famous Estonian island?",
                    ("Saaremaa", true), ("Gotland", false), ("Bornholm", false), ("Hawaii", false)));

                quiz2.Questions.Add(CreateSingleChoiceQuestion("When did Estonia join the European Union?",
                    ("2004", true), ("2007", false), ("2010", false), ("1994", false)));

                context.Quizzes.Add(quiz2);
            }

            await context.SaveChangesAsync();
        }

        // Вспомогательный метод для быстрого создания SingleChoiceQuestion с вариантами
        private static ChoiceQuestion CreateSingleChoiceQuestion(string text, params (string optionText, bool isCorrect)[] options)
        {
            var question = new ChoiceQuestion
            {
                Text = text
            };

            foreach (var (optionText, isCorrect) in options)
            {
                var answerOption = new AnswerOption
                {
                    Text = optionText,
                    IsCorrect = isCorrect
                };
                question.AnswerOptions.Add(answerOption);
                question.Options.Add(answerOption); // <-- синхронизация для UI
            }

            return question;
        }
    }
}

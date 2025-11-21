# Kaxut_demo

Kaxut_demo is a simple WPF quiz application inspired by Kahoot.  
The project allows the user to create a small quiz and then play it in a separate game window.

## Description

The application contains three main windows:

1. MainWindow
   - Used to create the quiz.
   - Supports up to 10 questions.
   - Each question contains four answer options and one correct answer.

2. GameWindow
   - Displays questions one by one.
   - Each question has a 15-second timer.
   - Buttons are highlighted (green for correct, red for incorrect).
   - Points are awarded for correct answers and remaining time.

3. ResultWindow
   - Shows total score and number of correct answers.
   - Allows starting a new quiz.

## Features

- Creating up to 10 questions
- Four answer options per question
- 15-second timer
- Basic UI animations
- Score calculation
- Results window

## How to run

1. Open the project in Visual Studio.
2. Build the solution.
3. Run the application (MainWindow starts first).
4. Create a quiz and press "Start".

## Purpose

This project serves as a simple demonstration of WPF:
window navigation, timers, animations, basic data models, and UI interaction.

using System;
using System.Collections.Generic;
using System.Diagnostics;

// Question base class
public abstract class Question
{
    public string Text { get; set; }
    public abstract bool CheckAnswer(string answer);
    public abstract string GetType(); // Added method to get question type
    public abstract string GetCorrectAnswer(); // Added method to get correct answer
}

// Multiple Choice Question
public class MultipleChoiceQuestion : Question
{
    public List<string> Choices { get; set; }
    public int CorrectChoiceIndex { get; set; }

    public override bool CheckAnswer(string answer)
    {
        // Compare user's answer to correct choice
        return answer.Equals(Choices[CorrectChoiceIndex], StringComparison.OrdinalIgnoreCase);
    }

    public override string GetType() // Implemented method to get question type
    {
        return "Multiple Choice";
    }

    public override string GetCorrectAnswer() // Implemented method to get correct answer
    {
        return Choices[CorrectChoiceIndex];
    }
}

// Open-Ended Question
public class OpenEndedQuestion : Question
{
    public string CorrectAnswer { get; set; }

    public override bool CheckAnswer(string answer)
    {
        // Compare user's answer to correct answer
        return answer.Equals(CorrectAnswer, StringComparison.OrdinalIgnoreCase);
    }

    public override string GetType() // Implemented method to get question type
    {
        return "Open-Ended";
    }

    public override string GetCorrectAnswer() // Implemented method to get correct answer
    {
        return CorrectAnswer;
    }
}

// True or False Question
public class TrueFalseQuestion : Question
{
    public bool CorrectAnswer { get; set; }

    public override bool CheckAnswer(string answer)
    {
        // Convert user's answer to boolean and compare to correct answer
        bool userAnswer;
        if (bool.TryParse(answer, out userAnswer))
        {
            return userAnswer == CorrectAnswer;
        }
        return false; // If user input is not true/false
    }

    public override string GetType() // Implemented method to get question type
    {
        return "True or False";
    }

    public override string GetCorrectAnswer() // Implemented method to get correct answer
    {
        return CorrectAnswer ? "True" : "False";
    }
}

// Quiz Game class
public class QuizGame
{
    private List<Question> questions = new List<Question>();
    private List<string> correctAnswers = new List<string>(); // Store correct answers

    public void AddQuestion(Question question)
    {
        questions.Add(question);
    }

    public void DeleteQuestion(int index)
    {
        if (index >= 0 && index < questions.Count)
        {
            questions.RemoveAt(index);
        }
    }

    public void EditQuestion(int index, Question updatedQuestion)
    {
        if (index >= 0 && index < questions.Count)
        {
            questions[index] = updatedQuestion;
        }
    }

    public void Play()
    {
        int score = 0;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        foreach (Question question in questions)
        {
            Console.WriteLine($"[{question.GetType()}] {question.Text}"); // Display question type

            string userAnswer = Console.ReadLine();

            if (question.CheckAnswer(userAnswer))
            {
                score++;
            }

            // Store correct answer
            correctAnswers.Add(question.GetCorrectAnswer());
        }

        stopwatch.Stop();
        Console.WriteLine($"Your score: {score}/{questions.Count}");
        Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalMinutes:F0} minute {stopwatch.Elapsed.Seconds} seconds");
    }

    public void ShowCorrectAnswers()
    {
        Console.WriteLine("Correct Answers:");
        for (int i = 0; i < questions.Count; i++)
        {
            Console.WriteLine($"{questions[i].Text} [{questions[i].GetType()}]: {correctAnswers[i]}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        QuizGame quizGame = new QuizGame();

        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Add a new question");
            Console.WriteLine("2. Delete a question");
            Console.WriteLine("3. Edit a question");
            Console.WriteLine("4. Play the quiz");
            Console.WriteLine("5. Show correct answers");
            Console.WriteLine("6. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddQuestion(quizGame);
                    break;
                case "2":
                    DeleteQuestion(quizGame);
                    break;
                case "3":
                    EditQuestion(quizGame);
                    break;
                case "4":
                    quizGame.Play();
                    break;
                case "5":
                    quizGame.ShowCorrectAnswers();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    static void AddQuestion(QuizGame quizGame)
    {
        Console.WriteLine("Select the type of question:");
        Console.WriteLine("1. Multiple Choice");
        Console.WriteLine("2. Open-Ended");
        Console.WriteLine("3. True or False");

        string typeChoice = Console.ReadLine();
        switch (typeChoice)
        {
            case "1":
                AddMultipleChoiceQuestion(quizGame);
                break;
            case "2":
                AddOpenEndedQuestion(quizGame);
                break;
            case "3":
                AddTrueFalseQuestion(quizGame);
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    static void AddMultipleChoiceQuestion(QuizGame quizGame)
    {
        MultipleChoiceQuestion question = new MultipleChoiceQuestion();
        Console.WriteLine("Enter the question:");
        question.Text = Console.ReadLine();
        question.Choices = new List<string>();
        Console.WriteLine("Enter the choices (Max 4):");
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Enter choice {i + 1}:");
            question.Choices.Add(Console.ReadLine());
        }
        Console.WriteLine("Enter the index of the correct choice:");
        int correctChoiceIndex;
        if (int.TryParse(Console.ReadLine(), out correctChoiceIndex) && correctChoiceIndex > 0 && correctChoiceIndex <= 4)
        {
            question.CorrectChoiceIndex = correctChoiceIndex - 1;
            quizGame.AddQuestion(question);
            Console.WriteLine("Question added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid correct choice index.");
        }
    }

    static void AddOpenEndedQuestion(QuizGame quizGame)
    {
        OpenEndedQuestion question = new OpenEndedQuestion();
        Console.WriteLine("Enter the question:");
        question.Text = Console.ReadLine();
        Console.WriteLine("Enter the correct answer:");
        question.CorrectAnswer = Console.ReadLine();
        quizGame.AddQuestion(question);
        Console.WriteLine("Question added successfully.");
    }

    static void AddTrueFalseQuestion(QuizGame quizGame)
    {
        TrueFalseQuestion question = new TrueFalseQuestion();
        Console.WriteLine("Enter the question:");
        question.Text = Console.ReadLine();
        Console.WriteLine("Enter the correct answer (true/false):");
        bool correctAnswer;
        if (bool.TryParse(Console.ReadLine(), out correctAnswer))
        {
            question.CorrectAnswer = correctAnswer;
            quizGame.AddQuestion(question);
            Console.WriteLine("Question added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter true or false.");
        }
    }

    static void DeleteQuestion(QuizGame quizGame)
    {
        Console.WriteLine("Enter the index of the question to delete:");
        int index;
        if (int.TryParse(Console.ReadLine(), out index))
        {
            quizGame.DeleteQuestion(index);
            Console.WriteLine("Question deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid index. Please enter a valid integer.");
        }
    }

    static void EditQuestion(QuizGame quizGame)
    {
        Console.WriteLine("Enter the index of the question to edit:");
        int index;
        if (int.TryParse(Console.ReadLine(), out index))
        {
            // Prompt user to enter a new question
            Console.WriteLine("Select the type of question:");
            Console.WriteLine("1. Multiple Choice");
            Console.WriteLine("2. Open-Ended");
            Console.WriteLine("3. True or False");

            string typeChoice = Console.ReadLine();
            switch (typeChoice)
            {
                case "1":
                    EditMultipleChoiceQuestion(quizGame, index);
                    break;
                case "2":
                    EditOpenEndedQuestion(quizGame, index);
                    break;
                case "3":
                    EditTrueFalseQuestion(quizGame, index);
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid index. Please enter a valid integer.");
        }
    }

    static void EditMultipleChoiceQuestion(QuizGame quizGame, int index)
    {
        MultipleChoiceQuestion question = new MultipleChoiceQuestion();
        Console.WriteLine("Enter the question:");
        question.Text = Console.ReadLine();
        question.Choices = new List<string>();
        Console.WriteLine("Enter the choices (Max 4):");
        for (int i = 0; i < 4; i++)
        {
            Console.WriteLine($"Enter choice {i + 1}:");
            question.Choices.Add(Console.ReadLine());
        }
        Console.WriteLine("Enter the index of the correct choice:");
        int correctChoiceIndex;
        if (int.TryParse(Console.ReadLine(), out correctChoiceIndex) && correctChoiceIndex > 0 && correctChoiceIndex <= 4)
        {
            question.CorrectChoiceIndex = correctChoiceIndex - 1;
            quizGame.EditQuestion(index, question);
            Console.WriteLine("Question edited successfully.");
        }
        else
        {
            Console.WriteLine("Invalid correct choice index.");
        }
    }

    static void EditOpenEndedQuestion(QuizGame quizGame, int index)
    {
        OpenEndedQuestion question = new OpenEndedQuestion();
        Console.WriteLine("Enter the question:");
        question.Text = Console.ReadLine();
        Console.WriteLine("Enter the correct answer:");
        question.CorrectAnswer = Console.ReadLine();
        quizGame.EditQuestion(index, question);
        Console.WriteLine("Question edited successfully.");
    }

    static void EditTrueFalseQuestion(QuizGame quizGame, int index)
    {
        TrueFalseQuestion question = new TrueFalseQuestion();
        Console.WriteLine("Enter the question:");
        question.Text = Console.ReadLine();
        Console.WriteLine("Enter the correct answer (true/false):");
        bool correctAnswer;
        if (bool.TryParse(Console.ReadLine(), out correctAnswer))
        {
            question.CorrectAnswer = correctAnswer;
            quizGame.EditQuestion(index, question);
            Console.WriteLine("Question edited successfully.");
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter true or false.");
        }
    }
}
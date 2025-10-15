using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;

namespace SnakeGame
{
    static class Program
    {
        // данные
        const int WIDTH = 20;
        const int HEIGHT = 20;

        static Random rnd = new Random();

        static List<(int sx, int sy)> snake = new List<(int sx, int sy)>();
        static (int fx, int fy) food;

        static string direction;
        static bool gameOver;
        static int difficulty;
        static int appleCounter;
        static DateTime startTime;


        // очистка консоли
        static void ClearConsole()
        {
            Console.Clear();
        }

        // считывание с клавиатуры
        static void ReadInput()
        {
            while (!gameOver)
            {
            var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        if (direction != "DOWN")
                            direction = "UP";
                        break;
                    case ConsoleKey.S:
                        if (direction != "UP")
                            direction = "DOWN";
                        break;
                    case ConsoleKey.A:
                        if (direction != "RIGHT")
                            direction = "LEFT";
                        break;
                    case ConsoleKey.D:
                        if (direction != "LEFT")
                            direction = "RIGHT";
                        break;

                    case ConsoleKey.E:
                    case ConsoleKey.Escape:
                        direction = "EXIT";
                        break;
                }
            }
        }

        // движение змейки
        static void SnakeMove()
        {
            var head = snake[0];
            (int nx, int ny) newHead = head;

            switch (direction)
            {
                case "UP":
                    newHead = (head.sx, head.sy - 1);
                    break;
                case "DOWN":
                    newHead = (head.sx, head.sy + 1);
                    break;
                case "LEFT":
                    newHead = (head.sx - 1, head.sy);
                    break;
                case "RIGHT":
                    newHead = (head.sx + 1, head.sy);
                    break;
            }

            // если врезались в стенку
            if (newHead.nx == 0 || newHead.nx == WIDTH - 1 || newHead.ny == 0 || newHead.ny == HEIGHT - 1)
            {
                gameOver = true;
                return;
            }

            // если врезались в себя
            if (snake.Contains(newHead))
            {
                gameOver = true;
                return;
            }

            // добавляем в начало новую голову
            snake.Insert(0, newHead);

            // проверка на еду
            if (newHead == food)
            {
                appleCounter++;
                SpawnFood();
            }
            else
            {
                snake.RemoveAt(snake.Count - 1);
            }

            if (direction == "EXIT")
            {
                gameOver = true;
                return;
            }    
        }

        // появление еды на карте
        static void SpawnFood()
        {
            while (true)
            {
                int fx = rnd.Next(1, WIDTH - 1);
                int fy = rnd.Next(1, HEIGHT - 1);

                bool onSnake = false;
                foreach (var part in snake)
                {
                    if (fx == part.sx && fy == part.sy)
                    {
                        onSnake = true;
                        break;
                    }
                }

                if (!onSnake)
                {
                    food = (fx, fy);
                    break;
                }
            }
        }

        // отрисовка всего
        static void Draw()
        {
            ClearConsole();

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (y == 0 || y == HEIGHT - 1)
                        Console.Write("#");
                    else
                    {
                        if (x == 0 || x == WIDTH - 1)
                            Console.Write("#");
                        else if ((x, y) == snake[0])
                            Console.Write("O");
                        else if (snake.Skip(1).Contains((x, y)))
                            Console.Write("o");
                        else if (x == food.fx && y == food.fy)
                            Console.Write("@");
                        else
                            Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }
        }

        // проверка на победу
        static void CheckWin()
        {
            if (snake.Count == (WIDTH - 1) * (HEIGHT - 1))
            {
                Console.WriteLine("Поздравляем! Вы стали самой длинной змейкой в мире!");
                gameOver = true;
                return;
            }
        }

        // отображение статистики
        static void ViewStats()
        {
            Console.WriteLine("===== Ваш результат =====");
            Console.WriteLine($"Длина змейки: {snake.Count}");
            Console.WriteLine($"Яблок съедено: {appleCounter}");
            Console.WriteLine($"Время игры: {(DateTime.Now - startTime).TotalSeconds:F1} секунд");
            Console.WriteLine("=========================");

            Console.WriteLine("Нажмите любую клавишу, чтобы продолжить");
            Console.ReadKey(true);
        }

        // начальное меню
        static void ShowMenu()
        {
            ClearConsole();
            Console.WriteLine("================================");
            Console.WriteLine("Добро пожаловать в игру Змейка!");
            Console.WriteLine("================================");
            Console.WriteLine("Управление: W A S D");
            Console.WriteLine("Выход: E/Escape");
            Console.WriteLine("================================");
            Console.WriteLine("1. Начать игру");
            Console.WriteLine("2. Выход");
            Console.WriteLine("Выберите пункт: ");

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "1")
                {
                    Game();
                    break;
                }
                else if (input == "2")
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Введите 1 или 2!"); 
                }
            }
        }

        // выбор уровня сложности
        static void ChooseDifficulty()
        {
            while (true)
            {
                ClearConsole();
                Console.WriteLine("================================");
                Console.WriteLine("Выберите уровень сложности: ");
                Console.WriteLine("1. Лёгкий");
                Console.WriteLine("2. Средний");
                Console.WriteLine("3. Сложный");
                Console.WriteLine("================================");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            difficulty = 300;
                            return;
                        case 2:
                            difficulty = 200;
                            return;
                        case 3:
                            difficulty = 100;
                            return;
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка ввода! Попробуйте снова.");
                }
            }
        }

        // основной цикл игры
        static void Game()
        {
            ChooseDifficulty();

            gameOver = false;
            direction = "UP";
            appleCounter = 0;
            snake.Clear();
            snake.Add((WIDTH / 2, HEIGHT / 2));

            SpawnFood();

            Thread Input = new Thread(ReadInput);
            Input.Start();

            startTime = DateTime.Now;

            while (!gameOver)
            {
                SnakeMove();
                Draw();
                CheckWin();
                Thread.Sleep(difficulty);
            }

            ViewStats();
        }

        static void Main()
        {
            while (true)
                ShowMenu();
        }
    }
}
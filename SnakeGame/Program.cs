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

        static string direction = "UP";
        static bool gameOver = false;


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
                int fx = rnd.Next(2, WIDTH);
                int fy = rnd.Next(2, HEIGHT);

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

        static void CheckWin()
        {
            if (snake.Count == (WIDTH - 1) * (HEIGHT - 1))
            {
                return;
            }
        }

        static void Main()
        {
            // ДОБАВИТЬ МЕНЮ + ВЫБОР СЛОЖНОСТИ

            snake.Clear();
            snake.Add((WIDTH / 2, HEIGHT / 2));

            SpawnFood();

            Thread Input = new Thread(ReadInput);
            Input.Start();

            while (!gameOver)
            {
                SnakeMove();
                Draw();
                Thread.Sleep(200);
            }

            // ДОБАВИТЬ СТАТИСТИКУ + CHECKWIN
        }
    }
}
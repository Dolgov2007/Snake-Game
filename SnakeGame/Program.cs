using System;

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


        // очистка консоли
        static void ClearConsole()
        {
            Console.Clear();
        }

        // считывание с клавиатуры
        static void ReadInput()
        {

        }

        // движение змейки
        static void SnakeMove()
        {
            
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
            snake.Clear();
            snake.Add((WIDTH / 2, HEIGHT / 2));

            SpawnFood();

            while (true)
            {
                Draw();
                Thread.Sleep(100);
            }
        }
    }
}
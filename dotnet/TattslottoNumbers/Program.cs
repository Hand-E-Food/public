using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace TattslottoNumbers
{
    class Program
    {

        /// <summary>
        /// The numbers in each game.
        /// </summary>
        private static List<List<int>> games = new List<List<int>>();

        /// <summary>
        /// The random number generator.
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Executes the main program.
        /// </summary>
        /// <param name="args">Not used.</param>
        static void Main(string[] args)
        {
            int BallNumbers  = int.Parse(ConfigurationManager.AppSettings["BallNumbers" ]);
            int BallsPerGame = int.Parse(ConfigurationManager.AppSettings["BallsPerGame"]);
            int BallsToDraw  = int.Parse(ConfigurationManager.AppSettings["BallsToDraw" ]);
            bool success;
            do
            {
                var balls = CreateBalls(BallNumbers, BallsToDraw);
                success = CreateGames(balls, BallsPerGame);
                if (!success)
                    games.Clear();
            }
            while (!success);
            SortGames();
            WriteGames();
            Console.WriteLine();
            Console.WriteLine("Press [Escape] to quit.");
            while (Console.ReadKey(true).Key != ConsoleKey.Escape) ;
        }

        /// <summary>
        /// Creates the set of balls to use.
        /// </summary>
        /// <param name="Numbers">The highest number on a ball.</param>
        /// <param name="ReuseNumbers">The number of times to use each number.</param>
        /// <returns>A list of balls.</returns>
        private static List<int> CreateBalls(int numbers, int balls)
        {
            var result = new List<int>(balls);
            while (balls > 0)
            {
                if (balls >= numbers)
                {   // If all numbers can be added to the result...
                    // Add one of every ball to the result.
                    for (int number = 1; number <= numbers; number++)
                    {
                        result.Add(number);
                    }
                    balls -= numbers;
                }
                else
                {   // If not all numbers can be added to the result...
                    // Create a barrel with one of each number.
                    var barrel = new List<int>(numbers);
                    for (int number = 1; number <= numbers; number++)
                    {
                        barrel.Add(number);
                    }
                    // Move random balls into the result.
                    while (balls > 0)
                    {
                        int number = barrel[random.Next(barrel.Count)];
                        barrel.Remove(number);
                        result.Add(number);
                        balls--;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Creates the games by drawing balls.
        /// </summary>
        /// <param name="balls">The collection of balls to draw from.</param>
        /// <param name="size">The number of balls to draw for each ticket.</param>
        /// <returns>True if all balls were used; otherwise, false.</returns>
        private static bool CreateGames(List<int> balls, int size)
        {
            List<int> game;
            List<int> bin = new List<int>();
            // Until there are no more balls...
            while (balls.Count > 0)
            {
                // Initialise a new game.
                game = new List<int>();
                // While the game does not have enough balls...
                while (game.Count < size)
                {
                    // Until an unpaired ball is drawn...
                    int ball;
                    bool paired = false;
                    do
                    {
                        // Draw a ball.
                        if (balls.Count == 0) return false;
                        ball = DrawBall(balls);
                        // Check whether this ball is already paired with any other ball already in this game.
                        paired =
                            game.Count > 0 
                            && game.Any((other) => IsPaired(ball, other));
                        if (paired)
                            bin.Add(ball);
                    }
                    while (paired);
                    // Add the ball to the game.
                    game.Add(ball);
                    // Put the bin balls back in the barrel.
                    balls.AddRange(bin);
                    bin.Clear();
                }
                // Add this game to the result.
                games.Add(game);
            }
            return true;
        }

        /// <summary>
        /// Randomly draws a ball from the collection of balls and removes it from the collection.
        /// </summary>
        /// <param name="balls">The collection of balls from which to draw.</param>
        /// <returns>The ball that was drawn.</returns>
        private static int DrawBall(List<int> balls)
        {
            int i = random.Next(balls.Count);
            int ball = balls[i];
            balls.RemoveAt(i);
            return ball;
        }

        /// <summary>
        /// Checks whether the two balls are already paired on another game row.
        /// </summary>
        /// <param name="ball1">The first ball.</param>
        /// <param name="ball2">The second ball.</param>
        /// <returns>True if the balls are the same or already paired; otherwise, false.</returns>
        private static bool IsPaired(int ball1, int ball2)
        {
            return ball1 == ball2
                || games.Any((game) => game.Contains(ball1) && game.Contains(ball2));
        }

        /// <summary>
        /// Sorts the balls in each game.
        /// </summary>
        private static void SortGames()
        {
            foreach (var game in games)
            {
                game.Sort();
            }
        }

        /// <summary>
        /// Writes the numbers for each game to the console.
        /// </summary>
        private static void WriteGames()
        {
            foreach (var game in games)
            {
                foreach (var ball in game)
                {
                    Console.Write("{0,2} ", ball);
                }
                Console.WriteLine();
            }
        }
    }
}

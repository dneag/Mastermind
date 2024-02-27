
namespace Mastermind
{
    public class Game
    {
        const int ATTEMPTS = 12;
        int attempt = 1;

        // Random 4 digit code with each digit between 1 and 6
        private readonly List<int> secretCode = new();

        // Run the game
        public void Start()
        {
            PrintIntro();
            MakeSecretCode();
            Play();
            Assess();
            PrintOutro();
        }

        private static void PrintIntro()
        {
            Console.WriteLine("\nIt's time to play...");
            Console.WriteLine("\n### MASTERMIND ###\n");
            Console.WriteLine("You have 12 attempts to guess the secret code!");
            Console.WriteLine("The code is 4 digits long and each digit is between 1 and 6.");
            Console.WriteLine("Each incorrect guess will trigger a response with +'s and/or -'s.");
            Console.WriteLine("Each '+' indicates that some digit matches the value and position of a digit in the code.");
            Console.WriteLine("Each '-' indicates that some digit matches the value but not position of a digit in the code.");
            Console.WriteLine("A '-' will never occur twice for the same guessed digit.");
            Console.WriteLine("\n...GO!\n");
        }

        private static void PrintOutro()
        {
            Console.WriteLine("Thanks for playing!\n");
        }

        // Create the secret code
        private void MakeSecretCode()
        {

            Random r = new();

            for (int i = 0; i < 4; ++i)
            {
                secretCode.Add(r.Next(1, 7));
            }
        }

        // Run the game loop
        private void Play()
        {
            while (attempt <= ATTEMPTS)
            {
                try
                {
                    List<int> guess = ReadGuess();
                    String response = CheckGuess(guess);

                    if (response == "++++")
                    {
                        Console.WriteLine("\n" + response + " !!!");
                        break;
                    }

                    Console.WriteLine(response + "\n Attempts remaining: " + (ATTEMPTS - attempt++));
                }
                catch
                {
                    Console.WriteLine("Invalid guess: enter a 4 digit value with each digit between 1 and 6");
                }
            }
        }

        // Determine win or loss, then display
        private void Assess()
        {
            if (attempt <= ATTEMPTS)
            {
                Console.WriteLine("\nYou solved it!\n");
            }
            else
            {
                Console.WriteLine("\nYou lose :(\n");
            }
        }

        // Read player guess and ensure it adheres to the rules, then parse it to an array
        private static List<int> ReadGuess()
        {
            String? input = Console.ReadLine();
            if (!System.Text.RegularExpressions.Regex.IsMatch(input, "^[1-6]{4}$"))
            {
                throw new Exception();
            }

            int guess = int.Parse(input);
            List<int> guessArray = new();

            for (int i = 3; i >= 0; --i)
            {
                int digit = (guess / (int)Math.Pow(10, i)) % 10;
                guessArray.Add(digit);
            }

            return guessArray;
        }

        // Check how well the guess matches the code and return a string representing this
        private String CheckGuess(List<int> guess)
        {
            String response = "";

            List<int> code = new(secretCode);

            // Check for match with both value and position, and remove any found matches
            for (int i = 0; i < guess.Count; i++)
            {
                if (guess[i] == code[i])
                {
                    response += "+";
                    guess.RemoveAt(i);
                    code.RemoveAt(i);
                    i--;
                }
            }

            // Check for value match with remaining digit values
            foreach (int digit in guess)
            {
                for (int i = 0; i < code.Count; i++)
                {
                    if (digit == code[i])
                    {
                        response += "-";
                        code.RemoveAt(i);
                        break;
                    }
                }
            }

            return response != "" ? response : "\"whiff\"";
        }
    }
}

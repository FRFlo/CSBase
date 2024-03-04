using System.Security;

namespace CSBase
{
    /// <summary>
    /// Outils pour demander des entrées à l'utilisateur.
    /// </summary>
    public static class Inputs
    {
        /// <summary>
        /// Demande à l'utilisateur d'entrer un string.
        /// </summary>
        /// <param name="indication">Indication à afficher à l'utilisateur</param>
        /// <param name="allowEmpty">Autorise l'utilisateur à entrer une valeur vide</param>
        /// <param name="log">Log la valeur entrée par l'utilisateur</param>
        /// <returns>La valeur entrée par l'utilisateur</returns>
        public static string GetString(string indication, bool allowEmpty = false, bool log = true)
        {
            Console.Write($"{indication}: ");
            string output;

            do
            {
                output = Console.ReadLine() ?? string.Empty;
                if (output == "" && !allowEmpty)
                {
                    Console.WriteLine("Veuillez entrer une valeur !");
                }
            } while (output == "" && !allowEmpty);

            if (log)
            {
                Logger.Debug(indication);
            }

            return output;
        }

        /// <summary>
        /// Demande à l'utilisateur d'entrer un string sécurisé.
        /// </summary>
        /// <param name="indication">Indication à afficher à l'utilisateur</param>
        /// <param name="allowEmpty">Autorise l'utilisateur à entrer une valeur vide</param>
        /// <param name="log">Log la valeur entrée par l'utilisateur</param>
        /// <param name="replacement">Caractère de remplacement</param>
        /// <returns>La valeur entrée par l'utilisateur</returns>
        /// <remarks>La valeur entrée par l'utilisateur n'est pas affichée dans la console.</remarks>
        public static SecureString GetSecureString(string indication, bool allowEmpty = false, bool log = true, char replacement = '*')
        {
            Console.Write($"{indication}: ");
            SecureString output = new();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (output.Length > 0)
                    {
                        output.RemoveAt(output.Length - 1);
                        Console.Write("\b \b");
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    output.AppendChar(key.KeyChar);
                    Console.Write(replacement);
                }
            }

            if (!allowEmpty && output.Length == 0)
            {
                Console.WriteLine("Veuillez entrer une valeur !");
                return GetSecureString(indication, allowEmpty, log);
            }

            Console.WriteLine();

            if (log)
            {
                Logger.Debug(indication + new string(replacement, output.Length));
            }

            return output;
        }
        /// <summary>
        /// Demande à l'utilisateur d'entrer un int.
        /// </summary>
        /// <param name="indication">Indication à afficher à l'utilisateur</param>
        /// <param name="allowEmpty">Autorise l'utilisateur à entrer une valeur vide</param>
        /// <param name="log">Log la valeur entrée par l'utilisateur</param>
        /// <returns>La valeur entrée par l'utilisateur</returns>
        public static int GetInt(string indication, bool allowEmpty = false, bool log = true)
        {
            Console.Write($"{indication}: ");
            string input;
            int output = 0;

            do
            {
                input = Console.ReadLine() ?? string.Empty;
                if (input == "" && !allowEmpty)
                {
                    Console.WriteLine("Veuillez entrer une valeur !");
                }
                else if (!int.TryParse(input, out output))
                {
                    Console.WriteLine("Veuillez entrer un nombre !");
                }
            } while (input == string.Empty && !allowEmpty);

            if (log)
            {
                Logger.Debug(indication);
            }

            return output;
        }
    }
}

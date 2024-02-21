using System.Text;

namespace CSBase
{
    /// <summary>
    /// Classe permettant de manipuler du texte.
    /// </summary>
    public static class TextManipulations
    {
        /// <summary>
        /// Permet de représenter la différence entre deux dates en texte.
        /// </summary>
        /// <param name="start">Date de début</param>
        /// <param name="end">Date de fin</param>
        /// <returns>La différence entre les deux dates en texte.</returns>
        public static string TimeDiffToString(DateTime start, DateTime end)
        {
            List<string> strings = [];
            TimeSpan duration = end - start;

            if (duration.Days > 0) strings.Add($"{duration.Days} jour{(duration.Days > 1 ? "s" : string.Empty)}");
            if (duration.Hours > 0) strings.Add($"{duration.Hours} heure{(duration.Hours > 1 ? "s" : string.Empty)}");
            if (duration.Minutes > 0) strings.Add($"{duration.Minutes} minute{(duration.Minutes > 1 ? "s" : string.Empty)}");
            if (duration.Seconds > 0) strings.Add($"{duration.Seconds} seconde{(duration.Seconds > 1 ? "s" : string.Empty)}");

            return string.Join(", ", strings);
        }
        /// <summary>
        /// Permet d'unir un tableau de texte en une seule chaîne de caractères avec un séparateur intermédiaire et un séparateur final.
        /// </summary>
        /// <param name="strings">Tableau de texte</param>
        /// <param name="separator">Séparateur intermédiaire</param>
        /// <param name="lastSeparator">Séparateur final</param>
        /// <returns>La chaîne de caractères unie.</returns>
        public static string Join(string[] strings, string separator, string lastSeparator)
        {
            if (strings.Length == 0) return string.Empty;
            if (strings.Length == 1) return strings[0];

            StringBuilder builder = new();

            for (int i = 0; i < strings.Length - 1; i++)
            {
                builder.Append(strings[i]);
                builder.Append(separator);
            }

            builder.Append(lastSeparator);
#if NET5_0_OR_GREATER
            builder.Append(strings[^1]);
#else
            builder.Append(strings[strings.Length - 1]);
#endif
            return builder.ToString();
        }
        /// <summary>
        /// Permet d'obtenir la similarité entre deux chaînes de caractères en prenant en compte les fautes de frappe.
        /// </summary>
        /// <param name="a">Première chaîne de caractères</param>
        /// <param name="b">Deuxième chaîne de caractères</param>
        /// <returns>La similarité entre les deux chaînes de caractères.</returns>
        /// <remarks>La similarité est comprise entre 0 et 1.</remarks>
        /// <remarks>Plus la similarité est proche de 1, plus les chaînes de caractères sont similaires.</remarks>
        /// <remarks>Plus la similarité est proche de 0, plus les chaînes de caractères sont différentes.</remarks>
        public static double GetSimilarity(string a, string b)
        {
            if (a == b) return 1;

            int length = Math.Max(a.Length, b.Length);
            int distance = DamerauLevenshteinDistance(a, b);

            return 1 - (double)distance / length;
        }
        /// <summary>
        /// Permet d'obtenir la distance de Damerau-Levenshtein entre deux chaînes de caractères.
        /// </summary>
        /// <param name="a">Première chaîne de caractères</param>
        /// <param name="b">Deuxième chaîne de caractères</param>
        /// <returns>La distance de Damerau-Levenshtein entre les deux chaînes de caractères.</returns>
        /// <remarks>La distance de Damerau-Levenshtein est le nombre minimal de modifications à effectuer sur une chaîne de caractères pour obtenir l'autre.</remarks>
        public static int DamerauLevenshteinDistance(string a, string b)
        {
            int[,] matrix = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++) matrix[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) matrix[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = a[i - 1] == b[j - 1] ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        matrix[i - 1, j] + 1,
                        Math.Min(
                            matrix[i, j - 1] + 1,
                            matrix[i - 1, j - 1] + cost
                            )
                        );

                    if (i > 1 && j > 1 && a[i - 1] == b[j - 2] && a[i - 2] == b[j - 1])
                    {
                        matrix[i, j] = Math.Min(
                            matrix[i, j],
                            matrix[i - 2, j - 2] + cost
                            );
                    }
                }
            }

            return matrix[a.Length, b.Length];
        }
        /// <summary>
        /// Permet de formater une chaîne de caractères en une chaîne de caractères avec des majuscules et des minuscules.
        /// </summary>
        /// <param name="text">Chaîne de caractères à formater</param>
        /// <returns>La chaîne de caractères formatée.</returns>
        public static string Format(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            StringBuilder builder = new(text.ToLower());

            builder[0] = char.ToUpper(builder[0]);

            for (int i = 1; i < builder.Length; i++)
            {
                if (builder[i - 1] == ' ') builder[i] = char.ToUpper(builder[i]);
            }

            return builder.ToString();
        }
        /// <summary>
        /// Permet de censurer une chaîne de caractères.
        /// </summary>
        /// <param name="input">Chaîne de caractères à censurer</param>
        /// <param name="censor">Chaîne de caractères à censurer</param>
        /// <param name="replacement">Caractère de remplacement</param>
        internal static string Censor(string input, string censor, char replacement = '*')
        {
            return input.Replace(censor, new string(replacement, censor.Length));
        }
        /// <summary>
        /// Permet de censurer une chaîne de caractères.
        /// </summary>
        /// <param name="input">Chaîne de caractères à censurer</param>
        /// <param name="censors">Chaînes de caractères à censurer</param>
        /// <param name="replacement">Caractère de remplacement</param>
        internal static string Censor(string input, IEnumerable<string> censors, char replacement = '*')
        {
            return censors.Aggregate(input, (current, censor) => Censor(current, censor, replacement));
        }
    }
}

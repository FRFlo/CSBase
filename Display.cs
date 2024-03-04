using System.Text;

namespace CSBase
{
    /// <summary>
    /// Classe permettant d'afficher des données de manière plus lisible.
    /// </summary>
    public static class Display
    {
        /// <summary>
        /// Permet d'afficher un Dictionnaire en alignant les clés et les valeurs avec des espaces.
        /// </summary>
        /// <param name="dictionary">Dictionnaire à afficher</param>
        /// <return>La représentation de l'affichage du dictionnaire</return>
        public static string Dictionary(this Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0) return string.Empty;

            int maxLength = dictionary.Keys.Max(key => key.Length);
            StringBuilder sb = new();

            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                sb.Append($"{pair.Key.PadRight(maxLength)}: {pair.Value}");
            }

            return sb.ToString();
        }
        /// <summary>
        /// Permet d'afficher un Dictionnaire en alignant les clés et les valeurs avec des espaces.
        /// </summary>
        /// <param name="dictionary">Dictionnaire à afficher</param>
        /// <return>La représentation de l'affichage du dictionnaire</return>
        public static string Dictionary(this Dictionary<string, IEnumerable<string>> dictionary)
        {
            int maxKeyLength = dictionary.Keys.Max(key => key.Length);
            int maxListValueLength = dictionary.Values.SelectMany(list => list).Max(value => value.ToString().Length);
            StringBuilder sb = new();

            foreach (var pair in dictionary)
            {
                string key = pair.Key.PadRight(maxKeyLength);

                var list = pair.Value.Select(value => value.ToString().PadRight(maxListValueLength)).ToArray();

                sb.AppendLine($"{key} : {string.Join(" ", list)}");
            }

            return sb.ToString();
        }
        /// <summary>
        /// Permet d'afficher un Dictionnaire en alignant les clés et les valeurs avec des espaces.
        /// </summary>
        /// <param name="list">Dictionnaire à afficher</param>
        /// <return>La représentation de l'affichage du dictionnaire</return>
        public static string Dictionary(this IEnumerable<IEnumerable<string>> list)
        {
            if (list == null || !list.Any()) return string.Empty;

            int columns = list.Max(list => list.Count());
            int[] maxColumnLengths = new int[columns];

            foreach (var collection in list)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (collection.Count() > col)
                    {
                        string value = collection.ElementAt(col)?.ToString() ?? "";
                        maxColumnLengths[col] = Math.Max(maxColumnLengths[col], value.Length);
                    }
                }
            }

            StringBuilder sb = new();

            foreach (var collection in list)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (collection.Count() > col)
                    {
                        string value = collection.ElementAt(col)?.ToString() ?? "";
                        if (value.Length == 0) continue;

                        sb.Append(value.PadRight(maxColumnLengths[col]));
                        sb.Append(' ');
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}

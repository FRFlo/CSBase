using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CSBase
{
    /// <summary>
    /// Classe permettant de logger des messages dans la console et dans un fichier.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Indique si le programme est en mode debug.
        /// </summary>
        public static bool IsDebug { get; set; } = false;
        /// <summary>
        /// Chemin du fichier de log. Par défaut, il est situé dans le dossier du programme.
        /// <list type="bullet">
        /// <item><description>En mode debug, il est situé dans le dossier du programme avec le nom "debug.log".</description></item>
        /// <item><description>En mode release, il est situé dans le dossier du programme avec le nom "production.log".</description></item>
        /// </list>
        /// </summary>
        public static string LogFilePath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(),
#if DEBUG
                                                                  "debug.log");
#else
                                                                  "production.log");
#endif
        /// <summary>
        /// Indique si les logs doivent être écrits dans le fichier de log.
        /// </summary>
        public static bool LogToFile { get; set; } = false;
        /// <summary>
        /// Nom de la source des logs.
        /// </summary>
        private static readonly string sourceName = Assembly.GetExecutingAssembly().GetName().Name ?? "CSBase Logger";

        /// <summary>
        /// Affiche un message dans la console et l'écrit dans le fichier de log.
        /// </summary>
        /// <param name="prefix">Préfixe du message indiquant le type de message</param>
        /// <param name="prefixColor">Couleur du texte du préfixe</param>
        /// <param name="message">Message à afficher dans la console</param>
        /// <param name="color">Couleur du texte du message</param>
        /// <param name="icon">Icone pour différencier le message</param>
        private static void Print(string prefix, ConsoleColor prefixColor, string message, ConsoleColor color = ConsoleColor.White, LoggerIcon? icon = null)
        {
            using EventSource eventSource = new(sourceName);

            EventLevel level = prefix switch
            {
                "DEBUG" => EventLevel.Verbose,
                "INFO" => EventLevel.Informational,
                "WARN" => EventLevel.Warning,
                "ERREUR" => EventLevel.Error,
                _ => EventLevel.Informational
            };

            eventSource.Write(prefix, new EventSourceOptions { Keywords = EventKeywords.None, Level = level }, new { Message = message });

            if (prefix == "DEBUG" && !IsDebug)
            {
                return;
            }

            StringBuilder sb = new();
#if NET5_0_OR_GREATER
            if (message.EndsWith('\n'))
            {
                message = message[..^1];
            }
#else
            if (message.EndsWith("\n"))
            {
                message = message.Substring(0, message.Length - 1);
            }
#endif

            foreach (string line in message.Split('\n'))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write($"{DateTime.Now:dd/MM/yyyy, HH:mm:fff} ");
                Console.BackgroundColor = prefixColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(prefix);
                if (icon != null)
                {
                    Console.BackgroundColor = icon.BackgroundColor;
                    Console.ForegroundColor = icon.ForegroundColor;
                    Console.Write($" {icon.Icon} ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = color;
                Console.WriteLine($" {line}");
                Console.ResetColor();

                sb.Append($"{DateTime.Now:dd/MM/yyyy, HH:mm:fff} {prefix} {line}\n");
            }

            if (LogToFile) File.AppendAllText(LogFilePath, sb.ToString(), Encoding.UTF8);

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            EventLog eventLog = new("Application")
            {
                Source = sourceName
            };

            eventLog.WriteEntry($"{prefix} {message}", level switch
            {
                EventLevel.Critical => EventLogEntryType.Error,
                EventLevel.Error => EventLogEntryType.Error,
                EventLevel.Warning => EventLogEntryType.Warning,
                EventLevel.Informational => EventLogEntryType.Information,
                EventLevel.Verbose => EventLogEntryType.Information,
                _ => EventLogEntryType.Information
            });
        }

        /// <summary>
        /// Affiche un message de debug dans la console et l'écrit dans le fichier de log.
        /// </summary>
        /// <param name="message">Message à afficher dans la console</param>
        /// <param name="icon">Icone à afficher avant le message</param>
        public static void Debug(string message, LoggerIcon? icon = null)
        {
            Print("DEBUG", ConsoleColor.DarkGreen, message, ConsoleColor.Green, icon);
        }

        /// <summary>
        /// Affiche un message d'information dans la console et l'écrit dans le fichier de log.
        /// </summary>
        /// <param name="message">Message à afficher dans la console</param>
        /// <param name="icon">Icone à afficher avant le message</param>
        public static void Info(string message, LoggerIcon? icon = null)
        {
            Print("INFO", ConsoleColor.DarkBlue, message, ConsoleColor.Blue, icon);
        }

        /// <summary>
        /// Affiche un message de warning dans la console et l'écrit dans le fichier de log.
        /// </summary>
        /// <param name="message">Message à afficher dans la console</param>
        /// <param name="icon">Icone à afficher avant le message</param>
        public static void Warn(string message, LoggerIcon? icon = null)
        {
            Print("WARN", ConsoleColor.DarkYellow, message, ConsoleColor.Yellow, icon);
        }

        /// <summary>
        /// Affiche un message d'erreur dans la console et l'écrit dans le fichier de log.
        /// </summary>
        /// <param name="message">Message à afficher dans la console</param>
        /// <param name="icon">Icone à afficher avant le message</param>
        public static void Error(string message, LoggerIcon? icon = null)
        {
            Print("ERREUR", ConsoleColor.DarkRed, message, ConsoleColor.Red, icon);
        }
    }

    /// <summary>
    /// Classe permettant de définir une icone pour les logs.
    /// </summary>
    /// <param name="icon">Texte unicode définissant l'icone</param>
    /// <param name="iconColor">Couleur du texte de l'icone</param>
    /// <param name="iconBackground">Couleur d'arrière plan de l'icone</param>
    public class LoggerIcon(string icon, ConsoleColor iconColor, ConsoleColor iconBackground)
    {
        /// <summary>
        /// Texte unicode définissant l'icone
        /// </summary>
        public string Icon { get; set; } = icon;
        /// <summary>
        /// Couleur du texte de l'icone
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; } = iconColor;
        /// <summary>
        /// Couleur d'arrière plan de l'icone
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; } = iconBackground;
    }
}

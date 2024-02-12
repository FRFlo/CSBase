using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace CSBase
{
    /// <summary>
    /// Classe d'extension pour les enums
    /// </summary>
    public static class EnumExtensions
    {

        /// <summary>
        /// Récupère la valeur d'un attribut d'une valeur d'un enum
        /// </summary>
        /// <typeparam name="T">Type de l'attribut</typeparam>
        /// <param name="value">Valeur de l'enum</param>
        /// <returns>Sa valeur</returns>
        public static T? GetAttribute<T>(this Enum value) where T : Attribute
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .FirstOrDefault()?
                        .GetCustomAttribute<T>();
        }
        /// <summary>
        /// Récupère la description d'une valeur d'un enum.
        /// </summary>
        /// <param name="value">Valeur de l'enum</param>
        /// <returns>Sa description</returns>
        public static string GetDescription(this Enum value)
        {
            return value.GetAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
        }
        /// <summary>
        /// Récupère la valeur <see cref="EnumMemberAttribute"/> d'une valeur d'un enum
        /// </summary>
        /// <returns>Sa valeur <see cref="EnumMemberAttribute"/></returns>
        public static string GetEnumMember(this Enum value)
        {
            return value.GetAttribute<EnumMemberAttribute>()?.Value ?? value.ToString();
        }
    }
}

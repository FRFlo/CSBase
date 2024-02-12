namespace CSBase
{
    /// <summary>
    /// Classe de base pour les singletons.
    /// </summary>
    /// <typeparam name="T">Type du singleton</typeparam>
    public abstract class Singleton<T> where T : new()
    {
        /// <summary>
        /// Lock pour éviter les accès concurrents.
        /// </summary>
        private static readonly object _padlock = new();
        /// <summary>
        /// Instance nullable de la classe.
        /// </summary>
        /// <remarks>N'est utilisé que pour définir l'instance</remarks>
        protected static T? _instance;

        /// <summary>
        /// Instance de la classe <typeparamref name="T"/>.
        /// </summary>
        internal static T Instance
        {
            get
            {
                lock (_padlock)
                {
                    return _instance ??= new T();
                }
            }
        }
    }
}
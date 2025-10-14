using EasyNetQ;
using System.Collections.Concurrent;
using System.Reflection;

namespace Rental.MessageBus.Serializers
{
    public class ShortTypeNameSerializer : ITypeNameSerializer
    {
        private const string Prefix = "rental.";

        private static readonly ConcurrentDictionary<string, Type> TypeCache = new();

        private static readonly Assembly[] RelevantAssemblies = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a =>
                !a.IsDynamic &&
                !a.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase) &&
                !a.FullName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase) &&
                !a.FullName.StartsWith("netstandard", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        /// <summary>
        /// Converts a .NET type to a short name used as the exchange name.
        /// </summary>
        public string Serialize(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return $"{Prefix}{type.Name}";
        }

        /// <summary>
        /// Converts a serialized name back to its .NET type.
        /// </summary>
        public Type DeSerialize(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new EasyNetQException("Type name cannot be null or empty.");

            // Remove prefix if present
            var cleanName = typeName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)
                ? typeName.Substring(Prefix.Length)
                : typeName;

            // Try cached value first
            if (TypeCache.TryGetValue(cleanName, out var cachedType))
                return cachedType;

            // Search across relevant assemblies
            foreach (var assembly in RelevantAssemblies)
            {
                try
                {
                    var type = assembly
                        .GetTypes()
                        .FirstOrDefault(t => string.Equals(t.Name, cleanName, StringComparison.Ordinal));

                    if (type != null)
                    {
                        TypeCache[cleanName] = type; // cache it
                        return type;
                    }
                }
                catch (ReflectionTypeLoadException)
                {
                    // Skip assemblies that cannot be reflected
                    continue;
                }
            }

            throw new EasyNetQException(
                $"Cannot resolve type '{typeName}'. Make sure the event class '{cleanName}' exists in loaded assemblies.");
        }
    }
}


using System.Collections.Concurrent;

namespace BureauHexagonal.Core.Enums
{
    public enum ProviderType
    {
        None = 0,
        ViaCep = 1,
        BrasilApi,
    }

    public static class ProviderTypeDescriptor
    {
        public static IReadOnlyDictionary<ProviderType, string> Value;
        public static IReadOnlyDictionary<string, ProviderType> ValueReverse;

        static ProviderTypeDescriptor()
        {
            var dir = new ConcurrentDictionary<ProviderType, string>();
            var valueReverse = new ConcurrentDictionary<string, ProviderType>();

            foreach (var item in Enum.GetValues<ProviderType>())
            {
                dir[item] = item.ToString();
                valueReverse[item.ToString()] = item;
            }

            Value = dir;
            ValueReverse = valueReverse;
        }
    }
}

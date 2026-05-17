using System.Collections.Concurrent;

namespace BureauHexagonal.Core.Enums
{
    public enum StorageType
    {
        None = 0,
        Postgres = 1,
        Dynamo = 2,
    }

    public static class StorageTypeDescriptor
    {
        public static IReadOnlyDictionary<StorageType, string> Value;
        public static IReadOnlyDictionary<string, StorageType> ValueReverse;

        static StorageTypeDescriptor()
        {
            var dir = new ConcurrentDictionary<StorageType, string>();
            var valueReverse = new ConcurrentDictionary<string, StorageType>();

            foreach (var item in Enum.GetValues<StorageType>())
            {
                dir[item] = item.ToString();
                valueReverse[item.ToString()] = item;
            }

            Value = dir;
            ValueReverse = valueReverse;
        }
    }
}

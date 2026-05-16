using System.Collections.Concurrent;

namespace BureauHexagonal.Core.Enums
{
    public enum BureauType
    {
        None = 0,
        CEP = 1,
        CPF = 2,
        CNPJ = 3,
    }

    public static class BureauTypeDescriptor
    {
        public static IReadOnlyDictionary<BureauType, string> Value;
        public static IReadOnlyDictionary<string, BureauType> ValueReverse;

        static BureauTypeDescriptor()
        {
            var dir = new ConcurrentDictionary<BureauType, string>();
            var valueReverse = new ConcurrentDictionary<string, BureauType>();

            foreach (var item in Enum.GetValues<BureauType>())
            {
                dir[item] = item.ToString();
                valueReverse[item.ToString()] = item;
            }

            Value = dir;
            ValueReverse = valueReverse;
        }
    }
}

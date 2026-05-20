namespace BureauHexagonal.Core.Utils
{
    public static class EnumUtils
    {
        public static bool IsValidEnum<T>(T value) where T : Enum
        {
            return Enum.IsDefined(typeof(T), value) && Convert.ToInt32(value) is not 0;
        }
    }
}

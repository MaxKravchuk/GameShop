using System;

namespace GameShop.BLL.Enums.Extensions
{
    public static class ToEnumExtension
    {
        public static TEnum ToEnum<TEnum>(this string value) where TEnum : struct, Enum
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enumerated type");
            }

            foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                if (enumValue.ToString().Equals(value, StringComparison.OrdinalIgnoreCase))
                {
                    return enumValue;
                }
            }

            throw new ArgumentException($"Invalid value '{value}' for enum type {typeof(TEnum).Name}");
        }
    }
}

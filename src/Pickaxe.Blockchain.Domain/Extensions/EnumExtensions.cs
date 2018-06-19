using System;
using System.ComponentModel;
using System.Reflection;

namespace Pickaxe.Blockchain.Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            // Get the Description attribute value for the enum value
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute =
                fieldInfo.GetCustomAttribute<DescriptionAttribute>(false);

            if (attribute != null)
            {
                return attribute.Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}

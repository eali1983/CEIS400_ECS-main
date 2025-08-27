using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This class is used to help insert the Enum choices into comboBoxes
// maintaining data integrity from both front and back ends. Also
// keeps the code cleaner, modular, and reuseable.
// These will be called under the Main Form class for the program
// at Initialization.

namespace CEIS400_ECS
{
    public static class EnumHelper
    {
        // Get a list of (Value, Description) pairs from an enum type
        public static List<EnumItem<T>> GetEnumList<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new EnumItem<T>(e, GetDescription(e)))
                .ToList();
        }

        // Get the description of an enum value, or the name it no description
        public static string GetDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public static List<KeyValuePair<string, T>> GetEnumDescriptions<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T))
                       .Cast<T>()
                       .Select(value =>
                       {
                           string description = value.ToString();
                           FieldInfo field = typeof(T).GetField(value.ToString());
                           DescriptionAttribute attr = field?.GetCustomAttribute<DescriptionAttribute>();
                           if (attr != null)
                               description = attr.Description;

                           return new KeyValuePair<string, T>(description, value);
                       })
                       .ToList();
        }

        public static void BindToComboBox<T>(ComboBox comboBox) where T : Enum
        {
            comboBox.DataSource = GetEnumList<T>();
            comboBox.DisplayMember = "Deascription";
            comboBox.ValueMember = "Value";
        }
    }

    public class EnumItem<T>
    {
        public T Value { get; }
        public string Description { get; }

        public EnumItem(T value, string description)
        {
            Value = value;
            Description = description;
        }

        public override string ToString() => Description;
    }
}

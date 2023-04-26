// Licensed under the MIT License. See LICENSE in the project root for license information.

using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace OpenAI.Extensions
{
    /// <summary>
    /// https://stackoverflow.com/questions/39855694/convert-empty-strings-to-null-with-json-net
    /// </summary>
    internal class EmptyToNullStringValueProvider : IValueProvider
    {
        private readonly PropertyInfo memberInfo;

        public EmptyToNullStringValueProvider(PropertyInfo memberInfo) => this.memberInfo = memberInfo;

        public object GetValue(object target)
        {
            var result = memberInfo.GetValue(target);

            if (memberInfo.PropertyType == typeof(string) &&
                result != null &&
                string.IsNullOrWhiteSpace(result.ToString()))
            {
                result = null;
            }

            return result;
        }

        public void SetValue(object target, object value)
        {
            if (memberInfo.PropertyType == typeof(string) &&
                value != null &&
                string.IsNullOrWhiteSpace(value.ToString()))
            {
                value = null;
            }

            memberInfo.SetValue(target, value);
        }
    }
}

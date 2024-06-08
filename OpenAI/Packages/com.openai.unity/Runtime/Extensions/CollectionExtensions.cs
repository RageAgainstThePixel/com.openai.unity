// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;

namespace OpenAI.Extensions
{
    internal static class CollectionExtensions
    {
        public static void Append<T>(this List<T> self, IReadOnlyList<T> other)
            where T : IAppendable<T>, new()
        {
            if (other == null)
            {
                return;
            }

            foreach (var otherItem in other)
            {
                if (otherItem == null) { continue; }

                if (otherItem.Index.HasValue)
                {
                    if (otherItem.Index + 1 > self.Count)
                    {
                        var newItem = new T();
                        newItem.Append(otherItem);
                        self.Insert(otherItem.Index.Value, newItem);
                    }
                    else
                    {
                        self[otherItem.Index.Value].Append(otherItem);
                    }
                }
                else
                {
                    var newItem = new T();
                    newItem.Append(otherItem);
                    self.Add(newItem);
                }
            }
        }
    }
}

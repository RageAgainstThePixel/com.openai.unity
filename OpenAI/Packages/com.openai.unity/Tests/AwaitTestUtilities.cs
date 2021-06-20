// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Threading.Tasks;

namespace OpenAI.Tests
{
    /// <summary>
    /// https://forum.unity.com/threads/async-await-in-unittests.513857/
    /// </summary>
    internal static class AwaitTestUtilities
    {
        public static IEnumerator Await(Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                if (task.Exception != null)
                {
                    throw task.Exception;
                }
            }
        }

        public static IEnumerator Await(Func<Task> taskDelegate)
            => Await(taskDelegate.Invoke());
    }
}

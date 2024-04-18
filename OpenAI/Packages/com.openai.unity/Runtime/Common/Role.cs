// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;

namespace OpenAI
{
    public enum Role
    {
        System = 1,
        Assistant,
        User,
        Memory,
        [Obsolete("Use Tool")]
        Function,
        Tool
    }
}

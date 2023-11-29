// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections.Generic;
using UnityEngine.Scripting;

namespace OpenAI
{
    [Preserve]
    public interface IListResponse<out TObject>
        where TObject : BaseResponse
    {
        [Preserve]
        IReadOnlyList<TObject> Items { get; }
    }
}

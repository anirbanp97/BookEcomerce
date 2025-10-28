using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Common
{
    /// <summary>
    /// Generic wrapper for returning paged or non-paged data.
    /// </summary>
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Simple response model with status and message.
    /// </summary>
    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResult Success(string? message = null)
            => new OperationResult { IsSuccess = true, Message = message ?? Constants.Msg_Success };

        public static OperationResult Fail(string? message = null)
            => new OperationResult { IsSuccess = false, Message = message ?? Constants.Msg_Failure };
    }
}

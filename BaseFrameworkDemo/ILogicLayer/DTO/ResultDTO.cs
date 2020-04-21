using System.Collections.Generic;

namespace ILogicLayer.DTO
{
    public class ResultDTO<T> where T : class, new()
    {
        public ResultDTO() { }

        public ResultDTO(IEnumerable<T> list, object sum, int pageIndex, int pageSize)
        {
            Lists = list;
            Sum = (sum == null ? 0 : (int)sum);
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        /// <summary>
        /// 总数量
        /// </summary>
        public int Sum { get; set; }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<T> Lists { get; set; }
    }
}
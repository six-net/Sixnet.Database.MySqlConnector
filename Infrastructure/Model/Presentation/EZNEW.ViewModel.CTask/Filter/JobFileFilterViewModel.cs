using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.ViewModel.CTask.Filter
{
    /// <summary>
    /// 任务工作文件查询信息
    /// </summary>
    public class JobFileFilterViewModel : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public List<long> Ids
        {
            get;
            set;
        }

        /// <summary>
        /// 工作
        /// </summary>
        public string Job
        {
            get;
            set;
        }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? CreateDate
        {
            get;
            set;
        }

        #endregion
    }
}
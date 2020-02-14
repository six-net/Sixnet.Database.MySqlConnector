using EZNEW.Framework.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZNEW.DTO.CTask.Query.Filter
{
    /// <summary>
    /// 工作分组查询信息
    /// </summary>
    public class JobGroupFilterDto : PagingFilter
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public List<string> Codes
        {
            get;
            set;
        }

        /// <summary>
        /// 排除编号
        /// </summary>
        public List<string> ExcludeCodes

        {
            get;
            set;
        }


        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }


        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort
        {
            get;
            set;
        }


        /// <summary>
        /// 上级
        /// </summary>
        public string Parent
        {
            get;
            set;
        }


        /// <summary>
        /// 根节点
        /// </summary>
        public string Root
        {
            get;
            set;
        }


        /// <summary>
        /// 等级
        /// </summary>
        public int? Level
        {
            get;
            set;
        }


        /// <summary>
        /// 说明
        /// </summary>
        public string Remark
        {
            get;
            set;
        }


        #endregion

        #region 数据加载

        public bool LoadParent
        {
            get; set;
        }

        #endregion
    }
}
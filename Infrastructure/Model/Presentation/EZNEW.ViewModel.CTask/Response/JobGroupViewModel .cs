using System;

namespace EZNEW.ViewModel.CTask.Response
{
    /// <summary>
    /// 工作分组
    /// </summary>
    public class JobGroupViewModel
    {
        #region	属性

        /// <summary>
        /// 编号
        /// </summary>
        public string Code
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
        public int Sort
        {
            get;
            set;
        }

        /// <summary>
        /// 上级
        /// </summary>
        public JobGroupViewModel Parent
        {
            get;
            set;
        }

        /// <summary>
        /// 根节点
        /// </summary>
        public JobGroupViewModel Root
        {
            get;
            set;
        }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level
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
    }
}
using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 工作任务
    /// </summary>
    [Serializable]
    [Entity("CTask_Job", "CTask", "工作任务")]
    public class JobEntity : BaseEntity<JobEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Id
        {
            get { return GetPropertyValue<string>("Id"); }
            set { SetPropertyValue("Id", value); }
        }

        /// <summary>
        /// 分组
        /// </summary>
        [EntityField(Description = "分组")]
        public string Group
        {
            get { return GetPropertyValue<string>("Group"); }
            set { SetPropertyValue("Group", value); }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [EntityField(Description = "名称")]
        public string Name
        {
            get { return GetPropertyValue<string>("Name"); }
            set { SetPropertyValue("Name", value); }
        }

        /// <summary>
        /// 任务类型
        /// </summary>
        [EntityField(Description = "任务类型")]
        public int Type
        {
            get { return GetPropertyValue<int>("Type"); }
            set { SetPropertyValue("Type", value); }
        }

        /// <summary>
        /// 执行类型
        /// </summary>
        [EntityField(Description = "执行类型")]
        public int RunType
        {
            get { return GetPropertyValue<int>("RunType"); }
            set { SetPropertyValue("RunType", value); }
        }

        /// <summary>
        /// 状态
        /// </summary>
        [EntityField(Description = "状态")]
        public int Status
        {
            get { return GetPropertyValue<int>("Status"); }
            set { SetPropertyValue("Status", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Description
        {
            get { return GetPropertyValue<string>("Description"); }
            set { SetPropertyValue("Description", value); }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        [EntityField(Description = "更新时间", RefreshDate = true)]
        public DateTime UpdateDate
        {
            get { return GetPropertyValue<DateTime>("UpdateDate"); }
            set { SetPropertyValue("UpdateDate", value); }
        }

        /// <summary>
        /// 任务路径
        /// </summary>
        [EntityField(Description = "任务路径")]
        public string JobPath
        {
            get { return GetPropertyValue<string>("JobPath"); }
            set { SetPropertyValue("JobPath", value); }
        }

        /// <summary>
        /// 任务文件名称
        /// </summary>
        [EntityField(Description = "任务文件名称")]
        public string JobFileName
        {
            get { return GetPropertyValue<string>("JobFileName"); }
            set { SetPropertyValue("JobFileName", value); }
        }

        #endregion
    }
}
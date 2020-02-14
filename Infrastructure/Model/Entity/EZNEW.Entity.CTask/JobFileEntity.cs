using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 任务文件
    /// </summary>
    [Serializable]
    [Entity("CTask_JobFile", "CTask", "任务文件")]
    public class JobFileEntity : BaseEntity<JobFileEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public long Id
        {
            get { return GetPropertyValue<long>("Id"); }
            set { SetPropertyValue("Id", value); }
        }

        /// <summary>
        /// 工作任务
        /// </summary>
        [EntityField(Description = "工作任务")]
        public string Job
        {
            get { return GetPropertyValue<string>("Job"); }
            set { SetPropertyValue("Job", value); }
        }

        /// <summary>
        /// 文件标题
        /// </summary>
        [EntityField(Description = "文件标题")]
        public string Title
        {
            get { return GetPropertyValue<string>("Title"); }
            set { SetPropertyValue("Title", value); }
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        [EntityField(Description = "文件路径")]
        public string Path
        {
            get { return GetPropertyValue<string>("Path"); }
            set { SetPropertyValue("Path", value); }
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [EntityField(Description = "添加时间")]
        public DateTime CreateDate
        {
            get { return GetPropertyValue<DateTime>("CreateDate"); }
            set { SetPropertyValue("CreateDate", value); }
        }

        #endregion
    }
}
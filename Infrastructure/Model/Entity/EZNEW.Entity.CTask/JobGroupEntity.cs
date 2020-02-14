using System;
using EZNEW.Develop.Entity;
using EZNEW.Framework.Extension;

namespace EZNEW.Entity.CTask
{
    /// <summary>
    /// 工作分组
    /// </summary>
    [Serializable]
    [Entity("CTask_JobGroup", "CTask", "工作分组")]
    public class JobGroupEntity : BaseEntity<JobGroupEntity>
    {
        #region	字段

        /// <summary>
        /// 编号
        /// </summary>
        [EntityField(Description = "编号", PrimaryKey = true)]
        public string Code
        {
            get { return GetPropertyValue<string>("Code"); }
            set { SetPropertyValue("Code", value); }
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
        /// 排序
        /// </summary>
        [EntityField(Description = "排序")]
        public int Sort
        {
            get { return GetPropertyValue<int>("Sort"); }
            set { SetPropertyValue("Sort", value); }
        }

        /// <summary>
        /// 上级
        /// </summary>
        [EntityField(Description = "上级")]
        public string Parent
        {
            get { return GetPropertyValue<string>("Parent"); }
            set { SetPropertyValue("Parent", value); }
        }

        /// <summary>
        /// 根节点
        /// </summary>
        [EntityField(Description = "根节点")]
        public string Root
        {
            get { return GetPropertyValue<string>("Root"); }
            set { SetPropertyValue("Root", value); }
        }

        /// <summary>
        /// 等级
        /// </summary>
        [EntityField(Description = "等级")]
        public int Level
        {
            get { return GetPropertyValue<int>("Level"); }
            set { SetPropertyValue("Level", value); }
        }

        /// <summary>
        /// 说明
        /// </summary>
        [EntityField(Description = "说明")]
        public string Remark
        {
            get { return GetPropertyValue<string>("Remark"); }
            set { SetPropertyValue("Remark", value); }
        }

        #endregion
    }
}
namespace OpenAuth.Generator.Common
{
    public class ColumnModel
    {
        /// <summary>
        /// 表Id
        /// </summary>
        public int TableId { get; set; }
        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public int RowId { get; set; }

        /// <summary>
        /// 列Id
        /// </summary>
        public int ColumnId { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 字段长度
        /// </summary>
        public int DataLenth { get; set; }
        /// <summary>
        /// 字段精度
        /// </summary>
        public int DataPrecision { get; set; }
        /// <summary>
        /// 小数位数
        /// </summary>
        public int DigitalNumber { get; set; }

        public string Description { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public int IsPrimeKey { get; set; }


        /// <summary>
        /// 是否可空 false 不可空 true 可空
        /// </summary>
        public int IsNullAble { get; set; }
    }
}

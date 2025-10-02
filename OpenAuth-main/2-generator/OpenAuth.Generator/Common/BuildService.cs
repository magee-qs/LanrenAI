using System.Text;

namespace OpenAuth.Generator.Common
{
    public class BuildService
    {
        private string Application;
        private string Dbcontext;
        private string Entity;
        private TableModel tableModel;

        private string EntityNamespace;

        private List<ColumnModel> columnList;

        private string _wwwroot;
        public BuildService(string application, string dbcontext, string entity,
            TableModel tableModel, List<ColumnModel> columns, string wwwroot)
        {
            this.Application = application;
            this.Dbcontext = dbcontext;
            this.Entity = entity;
            this.tableModel = tableModel;

            EntityNamespace = application + ".Domain";

            columnList = columns;

            _wwwroot = wwwroot;
        }

        public string BuildEntity()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CreateClassSummary(tableModel.TableDescription));
            sb.AppendLine("    [Table(\"" + tableModel.TableName + "\")]");
            //sb.AppendLine("    [SugarTable(\"" + tableModel.TableName + "\")]");

            var entityBase = GetExtendEntity(columnList);
            sb.AppendLine("    public class " + Entity + ":" + entityBase);
            sb.AppendLine("    {");
            //生成属性
            foreach (var column in columnList)
            {
                if (column.ColumnName.ToLower() == "id")
                    continue;


                sb.Append(CreatePropertySummary(column.Description));
                string validateAttribute = GetValidteAttribute(column);
                if (!string.IsNullOrEmpty(validateAttribute))
                    sb.Append(validateAttribute);
                sb.AppendLine(CreateProperty(column));
                sb.AppendLine();
            }
            sb.AppendLine("    }");

            Console.WriteLine(_wwwroot);
            string path = _wwwroot + "/template/Entity.txt";

            string content = System.IO.File.ReadAllText(path);

            content = content.Replace("$NameSpace$", EntityNamespace)
                             .Replace("$Content$", sb.ToString());

            return content;
        }

        private string CreateClassSummary(string summary)
        {
            if (string.IsNullOrEmpty(summary))
                summary = "";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("    /// <summary>");
            sb.AppendLine("    /// " + summary);
            sb.AppendLine("    /// 创建人: 麦吉小小");
            sb.AppendLine("    /// 创建时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sb.AppendLine("    ///</summary>");
            return sb.ToString();
        }

        private string CreatePropertySummary(string summary)
        {
            if (string.IsNullOrEmpty(summary))
                summary = "";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("        /// <summary>");
            sb.AppendLine("        ///" + summary);
            sb.AppendLine("        ///</summary>");
            sb.AppendLine("        [Description(\"" + summary + "\")]");
            return sb.ToString();
        }

        private string GetValidteAttribute(ColumnModel column)
        {
            StringBuilder sb = new StringBuilder();
            if (column.IsPrimeKey == 1)
                return sb.ToString();
            if (column.DataType == "varchar" || column.DataType == "nvarchar")
            {
                if (column.IsNullAble == 0)
                {
                    sb.AppendLine("        [Required]");
                }
                if (column.DataLenth > 0)
                {
                    sb.AppendLine("        [MaxLength(" + column.DataLenth + ")]");
                }
            }
            return sb.ToString();
        }

        private string CreateProperty(ColumnModel column)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("        public ");
            switch (column.DataType)
            {
                case "int":
                    sb.Append(" int").Append(GetNullableType(column.IsNullAble));
                    break;
                case "bit":
                    sb.Append(" bool").Append(GetNullableType(column.IsNullAble));
                    break;
                case "varchar":
                    sb.Append(" string").Append(GetNullableType(column.IsNullAble)); ;
                    break;
                case "nvarchar":
                    sb.Append(" string").Append(GetNullableType(column.IsNullAble)); ;
                    break;
                case "decimal":
                    sb.Append(" decimal").Append(GetNullableType(column.IsNullAble));
                    break;
                case "bigint":
                    sb.Append(" long").Append(GetNullableType(column.IsNullAble));
                    break;
                case "datetime":
                    sb.Append(" DateTime").Append(GetNullableType(column.IsNullAble));
                    break;
                case "real":
                    sb.Append(" float").Append(GetNullableType(column.IsNullAble));
                    break;
                case "float":
                    sb.Append(" double").Append(GetNullableType(column.IsNullAble));
                    break;
                case "timestamp":
                    sb.Append(" byte[]");
                    break;
                default:
                    sb.Append("string");
                    break;
            }
            sb.Append(" ").Append(column.ColumnName).Append(" { get; set; }");
            return sb.ToString();
        }

        private string GetNullableType(int nullable)
        {
            if (nullable == 1)
            {
                return "?";
            }
            else
            {
                return "";
            }
        }

        private string GetExtendEntity(List<ColumnModel> columns)
        {
            var column = columns.Where(t => t.ColumnName == "Id").FirstOrDefault();
            if (column == null)
                return "EntityString";

            if (column.DataType == "bigint")
            {
                return "EntityLong";
            }
            if (column.DataType == "int")
            {
                return "EntityInt";
            }
            return "EntityString";
        }
    }
}

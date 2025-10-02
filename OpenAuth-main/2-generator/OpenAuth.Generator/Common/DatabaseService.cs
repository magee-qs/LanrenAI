using Microsoft.Data.SqlClient;

namespace OpenAuth.Generator.Common
{
    public class DatabaseService
    {
        private string ConnectionString;
        public DatabaseService(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public List<TableModel> GetTableList()
        {

            string sqlText = string.Format(@"SELECT a.id TableId,a.name TableName,
                          (SELECT a1.value FROM sys.extended_properties a1 WHERE a1.major_id= a.id AND a1.minor_id=0   )TableDescription
                    FROM sysobjects  a 
                    WHERE xtype='U' ORDER BY a.name ASC ");


            var data = SqlHelper.Query<TableModel>(ConnectionString, sqlText);
            return data;
        }

        public TableModel GetTable(int Id)
        {
            return GetTableList().Where(t => t.TableId == Id).FirstOrDefault();
        }

        public List<ColumnModel> GetColumnList(int id)
        {

            string sqlText = string.Format(@" SELECT a.colid ColumnId,
                       a.id TableId, a.name ColumnName, a.length DataLenth, b.name DataType,
                       a.xprec DataPrecision, a.xscale DigitalNumber, a.isnullable IsNullAble, c.value Description,
                       (case when exists(
                                      SELECT 1 FROM sysobjects where xtype = 'PK' and parent_obj = a.id and name in (
                                     SELECT name FROM sysindexes WHERE indid in (SELECT indid FROM sysindexkeys WHERE id = a.id AND colid = a.colid))) 
                                     then 1 else 0 END)IsPrimeKey, 
                       (CASE WHEN d.FKName IS NOT nulL THEN 1 ELSE 0 END) IsForeighKey ,
                        d.FKName ,d.ParentTableId,d.ParentTableName,d.ParentColumnId
                FROM syscolumns a LEFT JOIN systypes b ON a.xtype = b.xtype AND a.usertype = b.usertype
                LEFT JOIN sys.extended_properties c ON a.id = c.major_id AND a.colid = c.minor_id
                LEFT JOIN
                (SELECT a1.name FKName, a1.object_id Id, a1.parent_object_id TableId, b1.parent_column_id ColumnId,
                      c1.id ParentTableId, c1.name ParentTableName, b1.referenced_column_id ParentColumnId
                FROM sys.foreign_keys a1  JOIN sys.foreign_key_columns b1 ON a1.object_id = b1.constraint_object_id
                JOIN sysobjects c1 ON b1.referenced_object_id = c1.id) d ON  a.id = d.TableId AND a.colid = d.ColumnId
                WHERE a.id = @TableId ");


            var list = SqlHelper.Query<ColumnModel>(ConnectionString, sqlText,
                new SqlParameter("@TableId", id));
            return list;

        }
    }
}

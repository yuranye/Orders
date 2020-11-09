using Dapper;

namespace Showcase.Orders
{
    public class PartialUpdateSqlBuilder: SqlBuilder
    {
        public SqlBuilder Set(string sql, dynamic parameters = null) =>
            AddClause("set", sql, parameters, " , ", "SET ", "\n", false);
    }
}
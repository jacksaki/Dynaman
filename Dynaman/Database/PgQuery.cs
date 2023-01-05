using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
namespace Dynaman {
    public class PgQuery : IDisposable {
        private static DbConnectionParameter _parameter;
        private NpgsqlConnection _conn;

        public PgQuery(DbConnectionParameter parameter) {
            _conn = new NpgsqlConnection(parameter.ConnectionString);
        }
        public PgQuery()  {
            _conn = new NpgsqlConnection(_parameter.ConnectionString);
        }

        public static void SetParameter(DbConnectionParameter parameter) {
            using(var query = new PgQuery(parameter)) {
                _parameter = parameter;
            }
        }

        public int ExecuteNonQuery(string sql, IDictionary<string, object> parameters) {
            using(var cmd = GenerateCommand(sql, parameters)) {
                return cmd.ExecuteNonQuery();
            }
        }
        public SqlResultRow GetFirstRow(string sql, IDictionary<string, object> parameters) {
            return GetSqlResult(sql, parameters).Rows.FirstOrDefault();
        }

        public NpgsqlTransaction BeginTransaction() {
            return _conn.BeginTransaction();
        }

        public SqlResult GetSqlResult(string sql, IDictionary<string, object> parameters) {
            using (var cmd = GenerateCommand(sql, parameters)) 
            using (var dr = cmd.ExecuteReader()){
                return new SqlResult(dr);
            }            
        }

        private NpgsqlCommand GenerateCommand(string sql, IDictionary<string, object> parameters) {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            if (parameters != null) {
                foreach(var p in parameters) {
                    cmd.Parameters.Add(new NpgsqlParameter(p.Key, p.Value));
                }
            }
            return cmd;
        }
            private NpgsqlParameter CreateParameter(string name, object value) {
            return new NpgsqlParameter(name, value);
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)
                }
                try {
                    _conn.Close();
                    _conn.Dispose();
                } finally {

                }
                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、ファイナライザーをオーバーライドします
                // TODO: 大きなフィールドを null に設定します
                disposedValue = true;
            }
        }

        // // TODO: 'Dispose(bool disposing)' にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします
        // ~PgQuery()
        // {
        //     // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
        //     Dispose(disposing: false);
        // }

        public void Dispose() {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
using FranciscoPech;
using Microsoft.Data.Sqlite;
using System;
using System.Threading.Tasks;

namespace NissanDemo.Models
{
    public class DBConnection:IDisposable
    {
        SqliteConnection conn;
        public DBConnection(bool connect=false)
        {
            conn = new SqliteConnection(@"Data Source=C:\ProgramData\nissan\DataBase.sqlite3;");
            if (connect)
                conn.Open();
        }
        public void Connect()
        {
            conn.Open();
        }
        public void Close()
        {
            conn?.Close();
        }
        public void Dispose()
        {
            conn?.Close();
            conn?.Dispose();
        }

        public async Task<Status> ExecuteAsync(string query, SqliteParameter[] parameters = null)
        {
            try
            {
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    await cmd.ExecuteNonQueryAsync();
                    return Status.OK();
                }
            }
            catch (Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }
        public async Task<Status> RequestObjectsAsync(string query, Action<SqliteDataReader> Callback, SqliteParameter[] parameters = null)
        {
            try
            {
                using (SqliteCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (SqliteDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (await dr.ReadAsync())
                            Callback(dr);
                    }

                    return Status.OK();
                }
            }
            catch (Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }
    }
}

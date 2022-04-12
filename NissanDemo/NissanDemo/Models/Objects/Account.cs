using FranciscoPech;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NissanDemo.Models.Objects
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal Balance { get; set; }
        public string Name { get; set; }

        public Account(int id, int userid, string name, decimal balance = 0.0m)
        {
            Id = id;
            UserId = userid;
            Name = name;
            Balance = balance;
        }

        private SqliteParameter[] GetParameters()
        {
            return new SqliteParameter[]
            {
                new SqliteParameter("@0", Id),
                new SqliteParameter("@1", UserId),
                new SqliteParameter("@2", Name),
                new SqliteParameter("@3", Balance)
            };
        }

        public async Task<Status> Save()
        {
            string query = "INSERT INTO account(user_id, name, balance) VALUES(@1,@2,@3)";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }
        public async Task<Status> UpdateAmount()
        {
            string query = "UPDATE account SET balance = @3 WHERE id=@0;";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }
        static public async Task<Request<List<Account>>> GetUserAccounts(int userid)
        {
            List<Account> accs = new List<Account>();
            string query = "SELECT * FROM account WHERE user_id = @0;";
            Status st = await Settings.Instance.Connection.RequestObjectsAsync(query, (dr) =>
            {
                Account acc = new Account(dr.GetInt32(0), dr.GetInt32(1), dr.GetString(3), dr.GetDecimal(2));
                accs.Add(acc);
            }, new SqliteParameter[] { new SqliteParameter("@0", userid) });
            return new Request<List<Account>>(st, accs);
        }
        static public async Task<Request<Account>> GetAccount(int id,int userid)
        {
            Account acc = null;
            string query = "SELECT * FROM account WHERE id = @0 and user_id = @1 LIMIT 1;";
            Status st = await Settings.Instance.Connection.RequestObjectsAsync(query, (dr) =>
            {
                acc = new Account(dr.GetInt32(0), dr.GetInt32(1), dr.GetString(3), dr.GetDecimal(2));
                
            }, new SqliteParameter[] { new SqliteParameter("@0", id), new SqliteParameter("@1", userid) });
            return new Request<Account>(st, acc);
        }
    }
}
//TABLE
/*
 CREATE TABLE Account (
    id      INTEGER      NOT NULL
                         UNIQUE
                         PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER      NOT NULL
                         REFERENCES user (id) ON DELETE CASCADE,
    balance DECIMAL      NOT NULL,
    name    VARCHAR (60) NOT NULL
);

 */
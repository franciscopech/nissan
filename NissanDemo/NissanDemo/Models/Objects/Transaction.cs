using FranciscoPech;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NissanDemo.Models.Objects
{
    public interface ITransaction
    {
        int Id { get; set; }
        DateTime Date { get; set; }
        decimal NewBalance { get; set; }
        decimal Amount { get; set; }
        int AccountId { get; set; }
        bool IsDeposit { get; set; }
    }
    public class Transaction: ITransaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal NewBalance { get; set; }
        public decimal Amount { get; set; }
        public int AccountId { get; set; }
        public bool IsDeposit { get; set; }

        public SqliteParameter[] GetParameters()
        {
            return new SqliteParameter[]
            {
                new SqliteParameter("@0", Id),
                new SqliteParameter("@1", Date),
                new SqliteParameter("@2", NewBalance),
                new SqliteParameter("@3", Amount),
                new SqliteParameter("@4", AccountId)
            };
        }
    }
    public class Withdrawal:Transaction
    {
        public Withdrawal(int id, DateTime date, decimal newbalance, decimal amount, int accountid)
        {
            Id = id;
            Date = date;
            NewBalance = newbalance;
            Amount = amount;
            AccountId = accountid;
            IsDeposit = false;
        }
        public async Task<Status> Save()
        {
            string query = "INSERT INTO withdrawal (new_balance,amount, account_id) VALUES(@2,@3,@4);";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }
        static public async Task<Request<List<Withdrawal>>> GetWithdrawal(int idaccount)
        {
            string query = "SELECT * FROM withdrawal WHERE account_id = @0;";
            List<Withdrawal> wds = new List<Withdrawal>();
            Status st = await Settings.Instance.Connection.RequestObjectsAsync(query, (dr) =>
            {
                Withdrawal wd = new Withdrawal(dr.GetInt32(0), dr.GetDateTime(1), dr.GetDecimal(2), dr.GetDecimal(3), dr.GetInt32(4));
                wds.Add(wd);
            }, new SqliteParameter[] {new SqliteParameter("@0",idaccount)});
            return new Request<List<Withdrawal>>(st, wds);
        }
    }


    public class Deposit:Transaction
    {
        public Deposit(int id, DateTime date, decimal newbalance, decimal amount, int accountid)
        {
            Id = id;
            Date = date;
            NewBalance = newbalance;
            Amount = amount;
            AccountId = accountid;
            IsDeposit = true;
        }
        public async Task<Status> Save()
        {
            string query = "INSERT INTO deposit (new_balance,amount, account_id) VALUES(@2,@3,@4);";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }
        static public async Task<Request<List<Deposit>>> GetDeposits(int idaccount)
        {
            string query = "SELECT * FROM deposit WHERE account_id = @0;";
            List<Deposit> dps = new List<Deposit>();
            Status st = await Settings.Instance.Connection.RequestObjectsAsync(query, (dr) =>
            {
                Deposit dp = new Deposit(dr.GetInt32(0), dr.GetDateTime(1), dr.GetDecimal(2), dr.GetDecimal(3), dr.GetInt32(4));
                dps.Add(dp);
            }, new SqliteParameter[] { new SqliteParameter("@0", idaccount) });
            return new Request<List<Deposit>>(st, dps);
        }
    }
}
//Tables
/*
 CREATE TABLE withdrawal (
    id          INTEGER  PRIMARY KEY AUTOINCREMENT
                         NOT NULL
                         UNIQUE,
    date        DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    new_balance DECIMAL  NOT NULL,
    amount      DECIMAL  NOT NULL,
    account_id INTEGER NOT NULL,
);

CREATE TABLE deposit (
    id          INTEGER  PRIMARY KEY AUTOINCREMENT
                         NOT NULL
                         UNIQUE,
    date        DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    new_balance DECIMAL  NOT NULL,
    amount      DECIMAL  NOT NULL,
    account_id INTEGER NOT NULL,
);
 */ 
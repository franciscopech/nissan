using FranciscoPech;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NissanDemo.Models.Objects;
using System.Linq;
using NissanDemo.Models.Attributes;

namespace NissanDemo.Controllers.Api
{
    [Route("api/transaction")]
    [ApiController]
    public class TransactionsApi : ControllerBase
    {
        [Route("set-withdrawal")]
        public async Task<Status> SetWithdrawal()
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Models.Objects.User usr = JsonManager.GetCurrentUser(Request);
                Request<Account> Racc = await Account.GetAccount(Convert.ToInt32(jsn.IdAccount), usr.Id);
                Status st = Status.Error("Usuario no permitido");
                if (Racc.OK && Racc.Result != null)
                {
                    Account acc = Racc.Result;
                    decimal ammount = Convert.ToDecimal(jsn.Amount);
                    acc.Balance -= ammount;
                    if (acc.Balance >= 0)
                    {
                        Withdrawal whd = new Withdrawal(-1, DateTime.Now, acc.Balance, ammount, acc.Id);
                        st = (await whd.Save());
                        if (st.Code == 0)
                        {
                            st = await acc.UpdateAmount();
                        }
                    }
                    else
                    {
                        st = Status.Error("Fondos insuficientes");
                    }
                }

                return st;
            }
            catch(Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }
        [Route("set-deposit")]
        public async Task<Status> SetDeposit()
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Models.Objects.User usr = JsonManager.GetCurrentUser(Request);
                Request<Account> Racc = await Account.GetAccount(Convert.ToInt32(jsn.IdAccount), usr.Id);
                Status st = Status.Error("Usuario no permitido");
                if (Racc.OK && Racc.Result != null)
                {
                    Account acc = Racc.Result;
                    decimal ammount = Convert.ToDecimal(jsn.Amount);
                    acc.Balance += ammount;
                    Deposit dpst = new Deposit(-1, DateTime.Now, acc.Balance, ammount, acc.Id);
                    st = (await dpst.Save());
                    if (st.Code == 0)
                    {
                        st = await acc.UpdateAmount();
                    }
                    else
                    {
                        st = Status.Error("Fondos insuficientes");
                    }
                }

                return st;
            }
            catch (Exception ex)
            {
                return Status.Error(ex.Message);
            }
        }


        [Route("get-transactions")]
        public async Task<Request<List<ITransaction>>> GetTransactions()
        {
            try
            {
                var jsn = await JsonManager.GetJsonPost(Request);
                Models.Objects.User usr = JsonManager.GetCurrentUser(Request);
                Request<Account> Racc = await Account.GetAccount(Convert.ToInt32(jsn.IdAccount), usr.Id);
                List<ITransaction> trns = new List<ITransaction>();
                Status st = Status.Error("Usuario no permitido");
                if (Racc.OK && Racc.Result != null)
                {
                    Account acc = Racc.Result;
                    var Twds = Withdrawal.GetWithdrawal(acc.Id);
                    var Tdps = Deposit.GetDeposits(acc.Id);
                    
                    var dps = await Tdps;
                    var wds = await Twds;

                    trns.AddRange(dps.Result);
                    trns.AddRange(wds.Result);

                    trns = trns.OrderBy(x => x.Date).ToList();
                    st = Status.OK();
                }
                return new Request<List<ITransaction>>(st, trns);
            }
            catch (Exception ex)
            {
                return new Request<List<ITransaction>>(Status.Error(ex.Message), null);
            }
        }
    }
}

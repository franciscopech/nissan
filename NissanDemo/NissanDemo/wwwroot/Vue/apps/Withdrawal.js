const transaction = new Vue({
    el: "#transaction",
    data:
    {
        Amount: 0.0,
        Accounts: [],
        IdSelectedAccount: -1,
        MaxAmount: 1
    },
    watch:
    {
        IdSelectedAccount: function () {
            let aux = this.Accounts.find(x => x.Id === this.IdSelectedAccount);
            if (aux != undefined)
                this.MaxAmount = aux.Balance;
        },
        Amount: function (val) {
            if (val > this.MaxAmount) {
                this.Amount = this.MaxAmount;
            }
        }
    },
    methods:
    {
        RefreshList: function () {
            this.Accounts = [];
            InvokeObject({}, "api/account/get-accounts", function (data) {
                let rslt = data.Result;
                for (let i = 0; i < rslt.length; i++) {
                    transaction.Accounts.push(rslt[i]);
                }
            });
        },
        Doit: function () {
            Invoke({ "Amount": this.Amount, "IdAccount": this.IdSelectedAccount }, "api/transaction/set-withdrawal", (data) => {
                window.location.replace("/Users/Index/");
            });
        }
    },
    created: function () {
            this.RefreshList();
    },
});
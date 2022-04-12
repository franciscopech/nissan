const transaction = new Vue({
    el: "#transaction",
    data:
    {
        Amount: 0.0,
        Accounts: [],
        IdSelectedAccount: -1,
        MaxAmount: 1
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
            Invoke({ "Amount": this.Amount, "IdAccount": this.IdSelectedAccount }, "api/transaction/set-deposit", (data) => {
                window.location.replace("/Users/Index/");
            });
        }
    },
    created: function () {
            this.RefreshList();
    },
});
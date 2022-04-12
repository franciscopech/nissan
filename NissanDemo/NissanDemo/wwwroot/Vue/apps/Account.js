const account = new Vue({
    el: "#create_acount",
    data: {
        Balance:0.0,
        Name:"",
    },
    methods: {
        save: function () {
            Invoke({ "Balance": this.Balance, "Name": this.Name }, "api/account/create-account", (data) => {
                window.location.replace("/");
            });
        }
    }
});


const detail = new Vue({
    el: "#movimientos",
    data: {
        IdAccount:0,
        Transitions: [],
    },
    methods: {
        RefreshWithdrawals: function () {
            InvokeObject({ "IdAccount": this.IdAccount }, "api/transaction/get-transactions", (data) => {
                let rslt = data.Result;
                for (let i = 0; i < rslt.length; i++) {
                    detail.Transitions.push(rslt[i]);
                }
            });
        }
    },
    created: function ()
    {
        let queryString = window.location.search;
        let urlParams = new URLSearchParams(queryString);
        this.IdAccount = urlParams.get('account');
        console.log(this.IdAccount);
        this.RefreshWithdrawals();
    }
});
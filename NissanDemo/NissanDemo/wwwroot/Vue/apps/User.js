const accounts = new Vue({
    el: "#accounts",
    data: {
        lista: []
    },
    methods: {
        RefreshList: function () {
            this.lista = [];
            InvokeObject({}, "api/account/get-accounts", function (data) {
                let rslt = data.Result;
                for (let i = 0; i < rslt.length; i++) {
                    accounts.lista.push(rslt[i]);
                }
            });
        }
    },
    created: function () {
        this.RefreshList();
    },
});
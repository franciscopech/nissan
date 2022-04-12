const create = new Vue({
    el: "#create",
    data: {
        user: { FirstName: "", LastName: "", Identification: "", Email: "", Password:"", Repassword:""}
    },
    computed: {
        DisableSave: function () {
            return this.user.Password.trim().length > 0 && this.user.Password == this.user.Repassword;
        }
    },
    methods: {
        Save: function () {
            Invoke(this.user, "api/user/CreateUser", (data) => {
                window.location.replace("/");
            });
        }
    }
});
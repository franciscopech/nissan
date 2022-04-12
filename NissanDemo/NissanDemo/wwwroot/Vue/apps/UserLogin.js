const login = new Vue({
    el: "#login",
    data: {
        Email: "",
        Password: ""
    },
    methods: {
        Login: function () {
            Invoke({ "email": this.Email, "password": this.Password }, "api/user/Login", (data) => {
                window.location.replace("/Users/index");
            });
        }
    }
});
using FranciscoPech;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace NissanDemo.Models.Objects
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Identification { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } //Podemos darle una capa de seguridad con hash o alguna encriptación

        public User(int id, string firstname, string lastname, string identification, string email)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Identification = identification;
            Email = email;
        }

        private SqliteParameter[] GetParameters()
        {
            return new SqliteParameter[]
            {
                new SqliteParameter("@0", Id),
                new SqliteParameter("@1", FirstName),
                new SqliteParameter("@2", LastName),
                new SqliteParameter("@3", Identification),
                new SqliteParameter("@4", Email),
                new SqliteParameter("@5", Password),
            };
        }

        static public async Task<Request<User>> Login(string email, string password)
        {
            User user = null;
            string psw = "";
            Status st = Status.Error("Credenciales inválidas");
            string query = "SELECT * FROM user WHERE email=@0;";
            var Tlgn = Settings.Instance.Connection.RequestObjectsAsync(query, (dr) =>
            {
                psw = dr.GetString(5);
                user = new User(dr.GetInt32(0), dr.GetString(1), dr.GetString(2), dr.GetString(3),dr.GetString(4));
            }, new SqliteParameter[] { new SqliteParameter("@0", email) });
            var lgn = await Tlgn;
            if (lgn.Code == 0 && user != null && psw == password)
            {
                st = Status.OK();
            }
            return new Request<User>( st, user);
        }

        /// <summary>
        /// Guarda el usuario en la DB
        /// </summary>
        /// <returns></returns>
        public async Task<Status> Save()
        {
            string query = "INSERT INTO user (fisrt_name, last_name, identification, email, password) VALUES(@1,@2,@3,@4,@5)";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }
        public async Task<Status> Update()
        {
            string query = "UPDATE user SET fisrt_name=@1, last_name=@2, identification = @3, email = @4 WHERE id=@0;";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }

        /// <summary>
        /// Eliminar el usuario de la DB
        /// </summary>
        /// <returns></returns>
        public async Task<Status> Delete()
        {
            string query = "DELETE FROM user WHERE id = @0";
            return await Settings.Instance.Connection.ExecuteAsync(query, this.GetParameters());
        }
    }
}
// TABLE 
/*
 CREATE TABLE user (
    id             INTEGER       PRIMARY KEY AUTOINCREMENT
                                 UNIQUE
                                 NOT NULL,
    fisrt_name     VARCHAR (80)  NOT NULL,
    last_name      VARCHAR (80)  NOT NULL,
    identification VARCHAR (100) UNIQUE
                                 NOT NULL,
    email          VARCHAR (150) NOT NULL
                                 UNIQUE,
    password       VARCHAR (200) NOT NULL
);

 */


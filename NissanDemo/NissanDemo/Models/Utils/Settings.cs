using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NissanDemo.Models;

namespace FranciscoPech
{
    public class Settings
    {
        private static Settings instance;
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                    instance = new Settings();
                return instance;
            }
        }
        public DBConnection Connection { get; set; }
        public Settings()
        {
            Connection = new DBConnection(true);
        }
    }
}

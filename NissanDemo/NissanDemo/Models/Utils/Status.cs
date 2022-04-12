using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FranciscoPech
{
    public class Status
    {
        public int Code { get; set; }
        public string Description { get; set; }

        public Status(int code, string description)
        {
            Code = code;
            Description = description;
        }
        static public Status OK()
        {
            return new Status(0, "OK");
        }
        static public Status ObjectExists()
        {
            return new Status(100, "OK");
        }
        static public Status Error(string e)
        {
            return new Status(500, e);
        }
        static public Status Alert(string message)
        {
            return new Status(400, message);
        }
    }
    public class Request<T>
    {
        public Status RequestStatus { get; set; }
        public T Result { get; set; }

        public Request(Status status, T result)
        {
            RequestStatus = status;
            Result = result;
        }
        public bool OK { get => RequestStatus.Code == 0; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.Exceptions
{
    public class AlreadeExistException : Exception
    {
        public AlreadeExistException(string msg) : base(msg)
        {

        }
    }
}

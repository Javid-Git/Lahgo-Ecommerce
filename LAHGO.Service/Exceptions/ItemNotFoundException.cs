using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public class ItemtNoteFoundException : Exception
        {
            public ItemtNoteFoundException(string msg) : base(msg)
            {

            }
        }
    }
}

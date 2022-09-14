using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.AccountVMs
{
    public class EmailVM
    {
        public string Server { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public int Port{ get; set; }
    }
}

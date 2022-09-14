using System;
using System.Collections.Generic;
using System.Text;

namespace LAHGO.Service.ViewModels.SettingVMs
{
    public class SettingListVM
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsDeleted { get; set; }
    }
}

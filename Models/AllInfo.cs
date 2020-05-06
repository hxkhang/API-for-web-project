using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestAPI.Models
{
    public class AllInfo
    {
        public string UserName { get; set; }
        public string EquipName { get; set; }
        public string EquipDes { get; set; }
        public string TypeName { get; set; }
    }
    public class EquipInfo
    {
        public string EquipName { get; set; }
        public string EquipDes { get; set; }
        public string EquipStatus { get; set; }
        public string TypeName { get; set; }
        public int EquipID { get; set; }
        public int TypeID { get; set; }
    }
    public class ListAllInfo
    {
        public List<AllInfo> AllInfos { get; set; }
        public List<EquipInfo> EquipInfos { get; set; }
    }
}

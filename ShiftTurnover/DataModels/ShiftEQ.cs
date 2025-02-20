using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShiftTurnover.DataModels
{
    public class ShiftEQ
    {
        public class Status
        {
            public int EQTagID { get; set; }
            public string Tag { get; set; }
            public string Comments { get; set; }
            public string Parent { get; set; }
            public string AcknowledgedByName { get; set; }
            public string AcknowledgeDate { get; set; }
            public string AddedBy { get; set; }
            public string UpdatedDate { get; set; }
        }
    }
}
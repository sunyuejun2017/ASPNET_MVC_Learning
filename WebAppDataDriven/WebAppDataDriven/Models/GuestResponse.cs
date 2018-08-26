using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppDataDriven.Models
{
    public class GuestResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? WillAttend { get; set; } // 这里的?表示WillAttend字段允许为空，可以是真，也可以是假，或者空

    }
}
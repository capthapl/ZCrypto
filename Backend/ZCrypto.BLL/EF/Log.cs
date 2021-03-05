using System;
using System.Collections.Generic;

#nullable disable

namespace ZCrypto.BLL.EF
{
    public partial class Log
    {
        public long Id { get; set; }
        public string Message { get; set; }
    }
}

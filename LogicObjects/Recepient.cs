﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicObjects
{
    public class Recepient
    {
        public string? RecepientName { get; set; }    

        public string? RecepientId { get; set; }

        public string? RecepientAccNo { get; set; }

        public long Relationship { get; set; }

        public Transactions transactions { get; set; } = new Transactions();

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ahlam.Extras
{
    public class FCMResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FCMResult> results { get; set; }
    }
}
﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Interfaces.Core
{
    public interface IRedisBase
    {
        public IDatabase client { get; set; }
    }
}

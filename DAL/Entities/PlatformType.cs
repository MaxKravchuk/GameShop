﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class PlatformType : BaseEntity
    {
        public string Type { get; set; }

        public ICollection<GamePlatformType> GamePlatformTypes { get; set; } = new List<GamePlatformType>();
    }
}

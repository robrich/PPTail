﻿using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTail.Interfaces
{
    public interface IOutputRepository
    {
        void Save(IEnumerable<SiteFile> files);
    }
}

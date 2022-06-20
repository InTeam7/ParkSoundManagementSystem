﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSoundManagementSystem.Core.Repositories
{
    public interface ITimeRepository
    {
        Task<DateTime> SetTime(DateTime time);
        Task<DateTime> GetTime();
        bool IsExist { get; }
    }
}

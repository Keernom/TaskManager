﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Abstractions
{
    public interface ICommonService<T>
    {
        bool Create(T model);
        bool Update(int id, T model);
        bool Delete(int id);
    }
}

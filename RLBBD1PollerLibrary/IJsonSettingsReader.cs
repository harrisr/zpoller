﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RLBBD1PollerLibrary
{
    public interface IJsonSettingsReader
    {
        T Load<T>() where T : class, new();
        T LoadSection<T>() where T : class, new();
        object Load(Type type);
        object LoadSection(Type type);
    }
}

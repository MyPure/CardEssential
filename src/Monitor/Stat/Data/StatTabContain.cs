using System;
using System.Collections.Generic;

namespace CardEssential.Monitor.Stat.Data;

[Serializable]
public class StatTabContain
{
    public string tabName;
    public List<string> statNames;
}
using System;
using System.Collections.Generic;
using System.Linq;
using CardEssential.Monitor.FileUtility;
using UnityEngine;

namespace CardEssential.Monitor.Stat.Data;

public class StatTabDataFile : BaseDataFile
{
    public List<StatTabContain> tabContains;
    public HashSet<string> favoriteStats;

    public override string DataName => "StatData";

    public override string DataFolderPath => "Monitor";

    public override bool Valid
    {
        get
        {
            return tabContains != null;
        }
    }

    public override BaseDataFile Default
    {
        get
        {
            var data = new StatTabDataFile();
            data.tabContains = new List<StatTabContain>();
            data.favoriteStats = new HashSet<string>();

            foreach (var pair in StatFilter.NameFilterDefault)
            {
                var tabName = pair.Key;
                var statNames = pair.Value.ToList();

                data.tabContains.Add(new StatTabContain()
                {
                    tabName = tabName,
                    statNames = statNames
                });
            }

            return data;
        }
    }
}
using System.Collections.Generic;

namespace CardEssential.Injector.Stat;

public class StatPack
{
    public InGameStat Stat { get; set; }
    public GameStat Model => Stat.StatModel;
    public StatPackRate Rate { get; set; }
    public StatPackValue Value { get; set; }

    public List<StatModifierSource> ModifierSources => Stat.ModifierSources;
    
    public List<StatStatus> CurrentStatuses => Stat.CurrentStatuses;
    public StatStatus CurrentStatus => Stat.AnyCurrentStatus(false);

    public string Description => Model.Description;

    public bool NotAtBase => MBSingleton<GameManager>.Instance && MBSingleton<GameManager>.Instance.NotInBase;

    public string Name => Model.GameName;
    public string DefaultName => Model.GameName.DefaultText;

    public StatPack(InGameStat stat)
    {
        Stat = stat;
        Value = new StatPackValue(this);
        Rate = new StatPackRate(this);
    }
}
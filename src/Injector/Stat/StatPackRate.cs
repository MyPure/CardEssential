namespace CardEssential.Injector.Stat;

public class StatPackRate
{
    private StatPack OwnerPack { get; set; }
    private InGameStat Stat => OwnerPack.Stat;

    public float CurrentRatePerTick
    {
        get => Stat.SimpleRatePerTick;
    }

    public float AtBaseModifiedRate
    {
        get => Stat.AtBaseModifiedRate;
    }
    public float CurrentBaseRate
    {
        get => Stat.CurrentBaseRate;
        set => Stat.CurrentBaseRate = value;
    }
    public float GlobalModifiedRate
    {
        get => Stat.GlobalModifiedRate;
    }

    public float BaseRatePerTick
    {
        get => Stat.StatModel.BaseRatePerTick;
    }
    
    public StatPackRate(StatPack ownerPack)
    {
        OwnerPack = ownerPack;
    }
}
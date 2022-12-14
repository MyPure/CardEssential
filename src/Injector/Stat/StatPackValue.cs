namespace CardEssential.Injector.Stat;

public class StatPackValue
{
    private StatPack OwnerPack { get; set; }
    private InGameStat Stat => OwnerPack.Stat;
    
    public float CurrentValue => Stat.SimpleCurrentValue;

    public float CurrentBaseValue
    {
        get => Stat.CurrentBaseValue;
        set => Stat.CurrentBaseValue = value;
    }
    public float MinValue => Stat.StatModel.MinMaxValue.x;
    public float MaxValue => Stat.StatModel.MinMaxValue.y;

    public float GlobalModifiedValue => Stat.GlobalModifiedValue;
    public float AtBaseModifiedValue => Stat.AtBaseModifiedValue;

    public float BaseValue
    {
        get => Stat.StatModel.BaseValue;
    }
    
    
    public StatPackValue(StatPack ownerPack)
    {
        OwnerPack = ownerPack;
    }
}
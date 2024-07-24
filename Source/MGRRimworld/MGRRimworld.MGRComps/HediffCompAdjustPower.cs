using System;
using Verse;

namespace MGRRimworld.MGRComps;

[StaticConstructorOnStartup]
internal class HediffCompAdjustPower : HediffComp
{
    private float power;

    private HediffCompProperties_AdjustPower Props => (HediffCompProperties_AdjustPower)props;

    public override string CompLabelPrefix => $"x{Power}";

    public float Power
    {
        get => power;
        set => power = value;
    }

    public override void CompPostPostAdd(DamageInfo? dinfo)
    {
        if (!dinfo.HasValue)
        {
            return;
        }

        var amount = dinfo.Value.Amount;
        if (!(amount > 1f))
        {
            return;
        }

        var num = (float)Math.Round(amount / 2400f * Props.efficiency, 2);
        power += num;
    }

    public override void CompExposeData()
    {
        base.CompExposeData();
        Scribe_Values.Look(ref power, "power");
    }
}
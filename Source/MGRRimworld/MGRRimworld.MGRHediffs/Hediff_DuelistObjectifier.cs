using Verse;

namespace MGRRimworld.MGRHediffs;

internal class Hediff_DuelistObjectifier : HediffWithComps
{
    public override string Label => def.label;

    public override bool ShouldRemove => false;

    public override void PostAdd(DamageInfo? dinfo)
    {
        base.PostAdd(null);
    }
}
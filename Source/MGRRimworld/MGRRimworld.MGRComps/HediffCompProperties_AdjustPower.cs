using Verse;

namespace MGRRimworld.MGRComps;

internal class HediffCompProperties_AdjustPower : HediffCompProperties
{
    public float efficiency;

    public HediffCompProperties_AdjustPower()
    {
        compClass = typeof(HediffCompAdjustPower);
    }
}
using Verse;

namespace MGRRimworld.MGRHediffs;

internal class Hediff_NanomachineCore : HediffWithComps
{
    public override string Label => def.label;

    public override bool ShouldRemove => false;

    public override void ExposeData()
    {
        base.ExposeData();
        if (Scribe.mode != LoadSaveMode.PostLoadInit || Part != null)
        {
            return;
        }

        Log.Error($"{GetType().Name} has null part after loading.");
        pawn.health.hediffSet.hediffs.Remove(this);
    }
}
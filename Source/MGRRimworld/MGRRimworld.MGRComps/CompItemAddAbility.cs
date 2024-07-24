using RimWorld;
using Verse;

namespace MGRRimworld.MGRComps;

internal class CompItemAddAbility : CompCauseHediff_Apparel
{
    public CompProperties_ItemAddAbility Props => (CompProperties_ItemAddAbility)props;

    public override void Notify_Equipped(Pawn pawn)
    {
        pawn.health.AddHediff(Props.hediff);
    }

    public override void Notify_Unequipped(Pawn pawn)
    {
        if (pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff) != null)
        {
            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(Props.hediff));
        }
    }
}
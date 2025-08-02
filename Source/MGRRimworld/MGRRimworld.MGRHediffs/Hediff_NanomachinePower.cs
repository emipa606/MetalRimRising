using MGRRimworld.MGRComps;
using RimWorld;
using Verse;

namespace MGRRimworld.MGRHediffs;

internal class Hediff_NanomachinePower : HediffWithComps
{
    public override HediffStage CurStage => new()
    {
        capMods =
        [
            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Consciousness,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Moving,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Sight,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Breathing,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Hearing,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Talking,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = MGRDefOf.MGRDefOf.Eating,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.Manipulation,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.BloodPumping,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = PawnCapacityDefOf.BloodFiltration,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            },

            new PawnCapacityModifier
            {
                capacity = MGRDefOf.MGRDefOf.Metabolism,
                offset = this.TryGetComp<HediffCompAdjustPower>().Power
            }
        ]
    };

    public override void PostAdd(DamageInfo? dinfo)
    {
        if (dinfo is { Amount: > 1f })
        {
            this.TryGetComp<HediffCompAdjustPower>().CompPostPostAdd(dinfo);
        }

        base.PostAdd(dinfo);
    }
}
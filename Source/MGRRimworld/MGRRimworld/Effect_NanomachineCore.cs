using System;
using System.Collections.Generic;
using System.Linq;
using MGRRimworld.MGRComps;
using MGRRimworld.MGRUtils;
using RimWorld;
using Verse;

namespace MGRRimworld;

[StaticConstructorOnStartup]
internal class Effect_NanomachineCore : Verb_LaunchProjectile
{
    protected readonly string letterLabel = "MGR.NanomachinesUnleashed".Translate();

    protected readonly string letterText = "MGR.NanomachinesUnleashedText".Translate();

    protected readonly string letterTitle = "MGR.NanomachinesUnleashed".Translate();
    private Pawn casterPawn;

    protected ChoiceLetter letter;

    private Map map;

    private List<IntVec3> targets;

    public override bool Available()
    {
        return true;
    }

    public override bool CanHitTargetFrom(IntVec3 root, LocalTargetInfo targ)
    {
        return true;
    }

    protected override bool TryCastShot()
    {
        casterPawn = CasterPawn;
        map = CasterPawn.Map;
        if (!casterPawn.IsColonist)
        {
            return false;
        }

        var power = Unleash();
        CastingEffect(power, casterPawn.Position, map, casterPawn);
        return true;
    }

    private float Unleash()
    {
        var result = 0f;
        if (map.GameConditionManager.GetActiveCondition(MGRDefOf.MGRDefOf.SolarFlare) == null)
        {
            SendLetter();
            map.GameConditionManager.RegisterCondition(
                GameConditionMaker.MakeCondition(MGRDefOf.MGRDefOf.SolarFlare, 30000));
        }

        var num = RemoveAllMapBatteriesCharge();
        if (!(num > 0f))
        {
            return result;
        }

        var value = default(DamageInfo);
        value.SetAmount(num);
        if (casterPawn.health.hediffSet.HasHediff(MGRDefOf.MGRDefOf.NanomachineCorePower))
        {
            var hediffCompAdjustPower = casterPawn.health.hediffSet
                .GetFirstHediffOfDef(MGRDefOf.MGRDefOf.NanomachineCorePower).TryGetComp<HediffCompAdjustPower>();
            hediffCompAdjustPower.CompPostPostAdd(value);
        }
        else
        {
            casterPawn.health.AddHediff(MGRDefOf.MGRDefOf.NanomachineCorePower, null, value);
        }

        result = (float)Math.Round(num / 600f * 0.15, 2);

        return result;
    }

    private float RemoveAllMapBatteriesCharge()
    {
        var allNetsListForReading = map.powerNetManager.AllNetsListForReading;
        var totalEnergy = 0f;
        allNetsListForReading.ForEach(delegate(PowerNet i)
        {
            if (i.batteryComps.Any(x => x.StoredEnergy > 0.0))
            {
                i.batteryComps.ForEach(delegate(CompPowerBattery j)
                {
                    totalEnergy += j.StoredEnergy;
                    j.DrawPower(j.StoredEnergy);
                    MGR_Lightning_Creator.DoStrike(j.parent.Position, map);
                });
            }
        });
        return totalEnergy;
    }

    public void CastingEffect(float power, IntVec3 center, Map map, Pawn pawn)
    {
        var num = (int)Math.Min(Math.Max(power, 0f), 54f);
        var val = 0f;
        if (power >= 54f)
        {
            val = power / num;
        }

        var num2 = (int)Math.Min(Math.Max(val, 0f), 3f);
        var list = new List<Thing> { pawn };
        var source = from c in GenRadial.RadialCellsAround(center, num, true)
            where c.IsValid && c.InBounds(map) && !c.Equals(center)
            select c;
        for (var i = 0; i < num; i++)
        {
            var intVec = source.RandomElement();
            MGR_Lightning_Creator.DoStrike(intVec, map, list);
            float num3 = 3 + num2;
            var flame = DamageDefOf.Flame;
            GenExplosion.DoExplosion(intVec, map, num3, flame, pawn, 5, 35f, null, null, null, null,
                ThingDefOf.Filth_Ash, 1f, 1, GasType.BlindSmoke, false, null, 0f, 1, 0f, false, null, list);
        }
    }

    private void SendLetter()
    {
        letter = LetterMaker.MakeLetter(letterLabel, letterText, LetterDefOf.NeutralEvent);
        letter.title = letterTitle;
        letter.lookTargets = new LookTargets(CasterPawn);
        Find.LetterStack.ReceiveLetter(letter);
    }
}
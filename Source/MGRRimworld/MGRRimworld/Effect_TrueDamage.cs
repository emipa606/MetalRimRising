using MGRRimworld.MGRMoteMaker;
using RimWorld;
using UnityEngine;
using Verse;

namespace MGRRimworld;

[StaticConstructorOnStartup]
internal class Effect_TrueDamage : Verb_LaunchProjectile
{
    private static readonly Material bladeMat = MaterialPool.MatFrom("UI/BloodMist", false);

    private bool casterIsColonist;

    private Map sourceMap;
    private Pawn sourcePawn;

    private IntVec3 targetLocation;

    private bool targetLocationIsValid;

    public override bool Available()
    {
        if (!CasterIsPawn)
        {
            return true;
        }

        var casterPawn = CasterPawn;
        return casterPawn.Faction == Faction.OfPlayer || verbProps.ai_ProjectileLaunchingIgnoresMeleeThreats ||
               !casterPawn.mindState.MeleeThreatStillThreat ||
               !casterPawn.mindState.meleeThreat.Position.AdjacentTo8WayOrInside(casterPawn.Position);
    }

    public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
    {
        if (base.ValidateTarget(target, showMessages) && target.HasThing && target.Thing.Faction != Faction.OfPlayer)
        {
            return target.Thing.Faction.HostileTo(Faction.OfPlayer);
        }

        return false;
    }

    protected override bool TryCastShot()
    {
        if (!currentTarget.HasThing || currentTarget.Thing.Map != caster.Map ||
            !CurrentTarget.Pawn.HostileTo(CasterPawn))
        {
            return false;
        }

        sourcePawn = CasterPawn;
        sourceMap = sourcePawn.Map;
        casterIsColonist = CasterPawn.IsColonist;
        targetLocation = CurrentTarget.Cell;
        targetLocationIsValid = targetLocation.IsValid;
        if (!targetLocationIsValid || !casterIsColonist)
        {
            return false;
        }

        CreateEffect(sourcePawn, FleckDefOf.ExplosionFlash, new Vector3(-0.5f, 0f, -0.5f), 3);
        sourcePawn.Position = targetLocation;
        SearchForTargets(targetLocation, 3f, sourceMap, sourcePawn);
        return true;
    }

    private static void CreateEffect(Pawn sourcePawn, FleckDef fleckDef, Vector3 offset, int scale = 1)
    {
        var dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(sourcePawn, fleckDef, offset, scale);
        dataAttachedOverlay.link.detachAfterTicks = 5;
        sourcePawn.Map.flecks.CreateFleck(dataAttachedOverlay);
    }

    private void SearchForTargets(IntVec3 center, float radius, Map map, Pawn pawn)
    {
        Pawn pawn2 = null;
        var enumerable = GenRadial.RadialCellsAround(center, radius, true);
        DrawBlade(center.ToVector3(), 1f);
        foreach (var item in enumerable)
        {
            FleckMaker.ThrowDustPuff(item, map, 0.2f);
            if (item.InBounds(map) && item.IsValid)
            {
                pawn2 = item.GetFirstPawn(map);
            }

            if (pawn2 == null || !pawn2.HostileTo(CasterPawn))
            {
                continue;
            }

            var num = GetWeaponDmg(pawn);
            var loc = pawn2.Position.ToVector3();
            if (Rand.Chance((float)(0.05 + (0.15 * pawn.skills.GetSkill(SkillDefOf.Melee).levelInt)) / 2f))
            {
                num *= 10;
                MoteMaker.ThrowText(pawn2.DrawPos, pawn2.Map, "MGR.CriticalHit".Translate());
                var dataAttachedOverlay =
                    FleckMaker.GetDataAttachedOverlay(pawn2, MGRDefOf.MGRDefOf.FlashHollow_Red, Vector3.zero, 5f);
                dataAttachedOverlay.link.detachAfterTicks = 5;
                pawn2.Map.flecks.CreateFleck(dataAttachedOverlay);
            }

            MGR_MoteMaker.ThrowCrossStrike(loc, map, 1f);
            MGR_MoteMaker.ThrowBloodSquirt(loc, map, 1.5f);
            DamageEntities(pawn, pawn2, null, num, DamageDefOf.Cut);
        }
    }

    private static void DrawBlade(Vector3 center, float magnitude)
    {
        var pos = center;
        pos.y = AltitudeLayer.MoteOverhead.AltitudeFor();
        var s = new Vector3(magnitude, magnitude, 1.5f * magnitude);
        var matrix = default(Matrix4x4);
        for (var i = 0; i < 6; i++)
        {
            float angle = Rand.Range(0, 360);
            matrix.SetTRS(pos, Quaternion.AngleAxis(angle, Vector3.up), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, bladeMat, 0);
        }
    }

    private static void DamageEntities(Pawn instigatorPawn, Pawn victim, BodyPartRecord hitPart, int amt,
        DamageDef type)
    {
        amt = (int)(amt * (double)Rand.Range(1f, 3f));
        var dinfo = new DamageInfo(type, amt, 100f, -1f, instigatorPawn, hitPart);
        dinfo.SetAllowDamagePropagation(false);
        try
        {
            victim.TakeDamage(dinfo);
        }
        catch
        {
            // ignored
        }
    }

    private static int GetWeaponDmg(Pawn pawn)
    {
        var primary = pawn.equipment.Primary;
        var num = primary.GetStatValue(StatDefOf.MeleeWeapon_AverageDPS, false) * 0.7f;
        var statValue = primary.GetStatValue(StatDefOf.MeleeWeapon_DamageMultiplier, false);
        var statValue2 = pawn.GetStatValue(StatDefOf.MeleeDPS, false);
        return Mathf.RoundToInt((float)(0.6000000238418579 * statValue * (statValue2 + (double)num)));
    }
}
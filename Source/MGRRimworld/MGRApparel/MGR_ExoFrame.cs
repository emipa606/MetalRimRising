using System.Collections.Generic;
using MGRApparel.MGRGizmo;
using MGRRimworld.MGRDefOf;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MGRApparel;

internal class MGR_ExoFrame : Apparel
{
    private const float MinDrawSize = 1.2f;

    private const float MaxDrawSize = 1.55f;

    private const float MaxDamagedJitterDist = 0.05f;

    private const int JitterDurationTicks = 8;

    private readonly float ApparelScorePerEnergyMax = 0.25f;

    private readonly float EnergyLossPerDamage = 0.033f;

    private readonly float EnergyOnReset = 0.2f;

    private readonly int StartingTicksToReset = 500;
    private float energy;

    private Vector3 impactAngleVect;

    private int KeepDisplayingTicks = 1000;

    private int lastAbsorbDamageTick = -9999;

    private int lastKeepDisplayTick = -9999;

    private int ticksToReset = -1;

    private float EnergyMax => this.GetStatValue(StatDefOf.EnergyShieldEnergyMax);

    private float EnergyGainPerTick => this.GetStatValue(StatDefOf.EnergyShieldRechargeRate) / 60f;

    public float Energy => energy;

    public ShieldState ShieldState => ticksToReset > 0 ? ShieldState.Resetting : ShieldState.Active;

    private bool ShouldDisplay
    {
        get
        {
            var wearer = Wearer;
            if (!wearer.Spawned || wearer.Dead || wearer.Downed)
            {
                return false;
            }

            if (!wearer.InAggroMentalState && !wearer.Drafted &&
                (!wearer.Faction.HostileTo(Faction.OfPlayer) || wearer.IsPrisoner))
            {
                return Find.TickManager.TicksGame < lastKeepDisplayTick + 1000;
            }

            return true;
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref energy, "energy");
        Scribe_Values.Look(ref ticksToReset, "ticksToReset", -1);
        Scribe_Values.Look(ref lastKeepDisplayTick, "lastKeepDisplayTick");
    }

    public override IEnumerable<Gizmo> GetWornGizmos()
    {
        foreach (var wornGizmo in base.GetWornGizmos())
        {
            yield return wornGizmo;
        }

        if (Find.Selector.SingleSelectedThing == Wearer)
        {
            yield return new MGR_Gizmo_ExoFrame
            {
                exoFrame = this
            };
        }
    }

    public override float GetSpecialApparelScoreOffset()
    {
        return EnergyMax * ApparelScorePerEnergyMax;
    }

    public override void Tick()
    {
        base.Tick();
        if (Wearer == null)
        {
            energy = 0f;
        }
        else if (ShieldState == ShieldState.Resetting)
        {
            ticksToReset--;
            if (ticksToReset <= 0)
            {
                Reset();
            }
        }
        else if (ShieldState == ShieldState.Active)
        {
            energy += EnergyGainPerTick;
            if (!(energy <= (double)EnergyMax))
            {
                energy = EnergyMax;
            }
        }
    }

    public override bool CheckPreAbsorbDamage(DamageInfo dinfo)
    {
        if (ShieldState != 0)
        {
            return false;
        }

        if (dinfo.Def == DamageDefOf.EMP)
        {
            energy = 0f;
            Break();
            return false;
        }

        energy -= dinfo.Amount * EnergyLossPerDamage;
        if (energy <= 0.0)
        {
            Break();
            if (dinfo.Weapon == null || dinfo.Weapon.IsMeleeWeapon ||
                dinfo.Weapon.defName.Equals(MGRDefOf.MeleeWeapon_MGR_Katana.defName) ||
                dinfo.Weapon.defName.Equals(MGRDefOf.MeleeWeapon_MGR_Katana_Jump.defName))
            {
                Recoil(dinfo);
            }

            return false;
        }

        AbsorbedDamage(dinfo);
        return true;
    }

    private static void Recoil(DamageInfo dinfo)
    {
        GenExplosion.DoExplosion(dinfo.Instigator.Position, dinfo.Instigator.Map, 3f, DamageDefOf.Smoke,
            dinfo.Instigator, 0, -1f, null, null, null, null, ThingDefOf.Filth_Dirt, 1f);
        var damageInfo = new DamageInfo(DamageDefOf.Bomb, 100f, 200f, -1f, dinfo.Instigator, dinfo.HitPart);
        damageInfo.SetAllowDamagePropagation(false);
        damageInfo.Instigator.TakeDamage(dinfo);
    }

    public void KeepDisplaying()
    {
        lastKeepDisplayTick = Find.TickManager.TicksGame;
    }

    private void AbsorbedDamage(DamageInfo dinfo)
    {
        SoundDefOf.EnergyShield_AbsorbDamage.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
        impactAngleVect = Vector3Utility.HorizontalVectorFromAngle(dinfo.Angle);
        var loc = Wearer.TrueCenter() + (impactAngleVect.RotatedBy(180f) * 0.5f);
        var num = Mathf.Min(10f, 2f + (dinfo.Amount / 10f));
        FleckMaker.Static(loc, Wearer.Map, FleckDefOf.ExplosionFlash, num);
        var num2 = (int)num;
        for (var i = 0; i < num2; i++)
        {
            FleckMaker.ThrowDustPuff(loc, Wearer.Map, Rand.Range(0.8f, MinDrawSize));
        }

        lastAbsorbDamageTick = Find.TickManager.TicksGame;
        KeepDisplaying();
    }

    private void Break()
    {
        MGRDefOf.EnergyShield_Broken.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
        FleckMaker.Static(Wearer.TrueCenter(), Wearer.Map, FleckDefOf.ExplosionFlash, 12f);
        for (var i = 0; i < 6; i++)
        {
            var loc = Wearer.TrueCenter() +
                      (Vector3Utility.HorizontalVectorFromAngle(Rand.Range(0, 360)) * Rand.Range(0.3f, 0.6f));
            FleckMaker.ThrowDustPuff(loc, Wearer.Map, Rand.Range(0.8f, MinDrawSize));
        }

        energy = 0f;
        ticksToReset = StartingTicksToReset;
    }

    private void Reset()
    {
        if (Wearer.Spawned)
        {
            SoundDefOf.EnergyShield_Reset.PlayOneShot(new TargetInfo(Wearer.Position, Wearer.Map));
            FleckMaker.ThrowLightningGlow(Wearer.TrueCenter(), Wearer.Map, 3f);
        }

        ticksToReset = -1;
        energy = EnergyOnReset;
    }

    public override void DrawWornExtras()
    {
    }

    public override bool AllowVerbCast(Verb verb)
    {
        return true;
    }
}
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace MGRRimworld.MGRUtils;

[StaticConstructorOnStartup]
internal static class MGR_Lightning_Creator
{
    public static Mesh boltMesh;

    public static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt");

    public static void DoStrike(IntVec3 strikeLoc, Map map, List<Thing> thingsToIgnore = null)
    {
        SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
        boltMesh = LightningBoltMeshPool.RandomBoltMesh;
        if (!strikeLoc.Fogged(map))
        {
            GenExplosion.DoExplosion(strikeLoc, map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null,
                ThingDefOf.Filth_Ash, 0f, 1, null, false, null, 0f, 1, 0f, false, null, thingsToIgnore);
            var loc = strikeLoc.ToVector3Shifted();
            for (var i = 0; i < 4; i++)
            {
                FleckMaker.ThrowSmoke(loc, map, 1.5f);
                FleckMaker.ThrowMicroSparks(loc, map);
                FleckMaker.ThrowLightningGlow(loc, map, 1.5f);
            }
        }

        var info = SoundInfo.InMap(new TargetInfo(strikeLoc, map));
        SoundDefOf.Thunder_OnMap.PlayOneShot(info);
        Graphics.DrawMesh(boltMesh, strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity,
            FadedMaterialPool.FadedVersionOf(LightningMat, 1f), 0);
    }
}
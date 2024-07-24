using UnityEngine;
using Verse;

namespace MGRRimworld.MGRMoteMaker;

public static class MGR_MoteMaker
{
    public static void ThrowCrossStrike(Vector3 loc, Map map, float scale)
    {
        if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
        {
            return;
        }

        var moteThrown = (MoteThrown)ThingMaker.MakeThing(MGRDefOf.MGRDefOf.MGR_Mote_CrossStrike);
        moteThrown.Scale = 1.9f * scale;
        moteThrown.rotationRate = 0f;
        moteThrown.exactRotation = Rand.Range(0, 3);
        moteThrown.exactPosition = loc;
        moteThrown.SetVelocity(Rand.Range(0, 360), Rand.Range(0.05f, 0.1f));
        GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
    }

    public static void ThrowBloodSquirt(Vector3 loc, Map map, float scale)
    {
        if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
        {
            return;
        }

        var moteThrown = (MoteThrown)ThingMaker.MakeThing(MGRDefOf.MGRDefOf.MGR_Mote_BloodSquirt);
        moteThrown.Scale = 1.9f * scale;
        moteThrown.rotationRate = Rand.Range(-60, 60);
        moteThrown.exactPosition = loc;
        moteThrown.SetVelocity(Rand.Range(0, 360), Rand.Range(1f, 2f));
        GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map);
    }
}
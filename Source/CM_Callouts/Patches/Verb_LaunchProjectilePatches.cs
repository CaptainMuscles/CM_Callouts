using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    [StaticConstructorOnStartup]
    public static class Verb_LaunchProjectilePatches
    {
        [HarmonyPatch(typeof(Verb_LaunchProjectile))]
        [HarmonyPatch("WarmupComplete", MethodType.Normal)]
        public static class Verb_LaunchProjectile_WarmupComplete
        {
            [HarmonyPostfix]
            public static void Postfix(Verb_LaunchProjectile __instance)
            {
                if (__instance.CasterPawn == null)
                    return;

                if (CalloutUtility.CanCalloutNow(__instance.CasterPawn) && CalloutUtility.CanCalloutAtTarget(__instance.CurrentTarget.Thing))
                {
                    CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
                    if (calloutTracker != null && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack))
                        CalloutUtility.AttemptRangedAttackCallout(__instance.CasterPawn, __instance);
                }
            }
        }
    }
}

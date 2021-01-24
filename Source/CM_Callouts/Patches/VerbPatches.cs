using System;
using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    [StaticConstructorOnStartup]
    public static class Verb_Patches
    {
        [HarmonyPatch(typeof(Verb))]
        [HarmonyPatch("TryStartCastOn", new Type[] { typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(bool), typeof(bool) })]
        public static class Verb_TryStartCastOn
        {
            [HarmonyPostfix]
            public static void Postfix(Verb __instance)
            {
                if (__instance as Verb_MeleeAttack == null || __instance.CasterPawn == null)
                    return;

                if (__instance.CurrentTarget.Thing is Pawn)
                {
                    CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
                    if (calloutTracker != null && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Melee_Attack))
                        CalloutUtility.AttemptMeleeAttackCallout(__instance.CasterPawn, __instance.CurrentTarget.Thing as Pawn);
                }
            }
        }
    }
}

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
    public static class Verb_MeleeAttack_Patches
    {
        public static PendingCallout pendingCallout = null;

        [HarmonyPatch(typeof(Verb_MeleeAttack))]
        [HarmonyPatch("CreateCombatLog", MethodType.Normal)]
        public static class Verb_MeleeAttack_CreateCombatLog
        {
            [HarmonyPostfix]
            public static void Postfix(Verb_MeleeAttack __instance, Func<ManeuverDef, RulePackDef> rulePackGetter)
            {
                pendingCallout = null;

                if (__instance.maneuver == null || __instance.tool == null)
                    return;

                if (__instance.CurrentTarget.Thing is Pawn && Rand.Chance(CalloutMod.settings.baseCalloutChance))
                {
                    // Ignore dodge for now since it already throws a text mote
                    if (rulePackGetter(__instance.maneuver) == __instance.maneuver.combatLogRulesDodge)
                        return;

                    // If logging a hit, do the callout when we resolve the damage result
                    if (rulePackGetter(__instance.maneuver) == __instance.maneuver.combatLogRulesHit)
                    {
                        // This will get resolved in a DamageWorker.DamageResult.AssociateWithLog patch so we will know the result and don't have to transpile
                        pendingCallout = new PendingCallout(__instance.CasterPawn, __instance.CurrentTarget.Thing as Pawn);
                    }
                    else if (rulePackGetter(__instance.maneuver) == __instance.maneuver.combatLogRulesMiss)
                    {
                        // TODO: Melee miss callout
                    }
                }
            }
        }
    }
}

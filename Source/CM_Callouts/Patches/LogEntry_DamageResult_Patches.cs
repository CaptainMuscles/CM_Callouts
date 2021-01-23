using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    [StaticConstructorOnStartup]
    public static class LogEntry_DamageResult_Patches
    {
        [HarmonyPatch(typeof(LogEntry_DamageResult))]
        [HarmonyPatch("FillTargets", MethodType.Normal)]
        public static class LogEntry_DamageResult_FillTargets
        {
            [HarmonyPostfix]
            public static void Postfix(LogEntry_DamageResult __instance, List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
            {
                if (deflected)
                    return;


                // This log entry is linked to a melee attack with a pending callout
                if (__instance as BattleLogEntry_MeleeCombat != null && Verb_MeleeAttack_Patches.pendingCallout != null)
                {
                    try
                    {
                        MethodInfo getDamagedBody = __instance.GetType().GetMethod("DamagedBody", BindingFlags.NonPublic | BindingFlags.Instance);
                        BodyDef damagedBody = getDamagedBody?.Invoke(__instance, new object[] { }) as BodyDef;
                        CalloutUtility.AttemptMeleeAttackLandedCallout(Verb_MeleeAttack_Patches.pendingCallout, damagedBody, recipientParts, recipientPartsDestroyed);

                        ThrowDestroyedPartMotes(Verb_MeleeAttack_Patches.pendingCallout.recipient, recipientParts, recipientPartsDestroyed);
                    }
                    finally
                    {
                        Verb_MeleeAttack_Patches.pendingCallout = null;
                    }
                }
                // This log entry is linked to a melee attack with a pending callout
                else if (__instance as BattleLogEntry_RangedImpact != null && BattleLogEntry_RangedImpact_Patches.pendingCallout != null)
                {
                    try
                    {
                        // Only if we hit our original target to start with
                        if (BattleLogEntry_RangedImpact_Patches.pendingCallout.recipient == BattleLogEntry_RangedImpact_Patches.pendingCallout.originalTarget)
                        {
                            MethodInfo getDamagedBody = __instance.GetType().GetMethod("DamagedBody", BindingFlags.NonPublic | BindingFlags.Instance);
                            BodyDef damagedBody = getDamagedBody?.Invoke(__instance, new object[] { }) as BodyDef;
                            CalloutUtility.AttemptRangedAttackImpactCallout(BattleLogEntry_RangedImpact_Patches.pendingCallout, damagedBody, recipientParts, recipientPartsDestroyed);
                        }

                        ThrowDestroyedPartMotes(BattleLogEntry_RangedImpact_Patches.pendingCallout.recipient, recipientParts, recipientPartsDestroyed);
                    }
                    finally
                    {
                        BattleLogEntry_RangedImpact_Patches.pendingCallout = null;
                    }
                }
            }

            private static void ThrowDestroyedPartMotes(Pawn pawn, List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed)
            {
                if (!CalloutMod.settings.showWounds || !pawn.SpawnedOrAnyParentSpawned)
                    return;

                Vector3 thingVector3 = pawn.SpawnedParentOrMe.DrawPos;
                Map thingMap = pawn.SpawnedParentOrMe.Map;

                for (int i = 0; i < recipientPartsDestroyed.Count; ++i)
                {
                    if (recipientPartsDestroyed[i])
                    {
                        MoteMaker.ThrowText(thingVector3, thingMap, recipientParts[i].def.label, Color.magenta);
                    }
                    else
                    {
                        Color moteColor = HealthUtility.GetPartConditionLabel(pawn, recipientParts[i]).Second;
                        MoteMaker.ThrowText(thingVector3, thingMap, recipientParts[i].def.label, moteColor);
                    }
                }
            }
        }
    }
}

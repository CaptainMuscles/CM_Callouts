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
                if (__instance as BattleLogEntry_MeleeCombat != null && Verb_MeleeAttack_Patches.pendingCallout != null)
                {
                    try
                    {
                        MethodInfo getDamagedBody = __instance.GetType().GetMethod("DamagedBody", BindingFlags.NonPublic | BindingFlags.Instance);
                        BodyDef damagedBody = getDamagedBody?.Invoke(__instance, new object[] { }) as BodyDef;
                        CalloutUtility.AttemptMeleeAttackLandedCallout(Verb_MeleeAttack_Patches.pendingCallout, damagedBody, recipientParts, recipientPartsDestroyed);

                    }
                    finally
                    {
                        Verb_MeleeAttack_Patches.pendingCallout = null;
                    }
                }
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    [StaticConstructorOnStartup]
    public static class DamageWorker_DamageResult_Patches
    {
        [HarmonyPatch(typeof(DamageWorker.DamageResult))]
        [HarmonyPatch("AssociateWithLog", MethodType.Normal)]
        public static class DamageWorker_DamageResult_AssociateWithLog
        {
            [HarmonyPostfix]
            public static void Postfix(DamageWorker.DamageResult __instance)
            {
                if (__instance.deflected)
                    return;

                Pawn hitPawn = __instance.hitThing as Pawn;
                if (hitPawn != null && !__instance.parts.NullOrEmpty())
                {
                    List<BodyPartRecord> recipientPartsDamaged = __instance.parts.Distinct().ToList();
                    List<bool> recipientPartsDestroyed = recipientPartsDamaged.Select((BodyPartRecord part) => hitPawn.health.hediffSet.GetPartHealth(part) <= 0f).ToList();

                    if (CalloutUtility.pendingCallout != null)
                    {
                        CalloutUtility.pendingCallout.FillBodyPartInfo(hitPawn.RaceProps.body, recipientPartsDamaged, recipientPartsDestroyed);
                        CalloutUtility.pendingCallout.AttemptCallout();

                        CalloutUtility.pendingCallout = null;
                    }

                    ThrowDestroyedPartMotes(hitPawn, recipientPartsDamaged, recipientPartsDestroyed);
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
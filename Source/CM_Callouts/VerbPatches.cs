﻿//using System.Collections.Generic;

//using UnityEngine;
//using HarmonyLib;
//using RimWorld;
//using Verse;
//using Verse.AI;

//namespace CM_Callouts
//{
//    [StaticConstructorOnStartup]
//    public static class Verb_Patches
//    {
//        [HarmonyPatch(typeof(Verb))]
//        [HarmonyPatch("WarmupComplete", MethodType.Normal)]
//        public static class Verb_WarmupComplete
//        {
//            [HarmonyPostfix]
//            public static void Postfix(Verb __instance)
//            {
//                if (__instance as Verb_MeleeAttack == null || __instance.CasterPawn == null)
//                    return;

//                if (__instance.CurrentTarget.Thing is Pawn && Rand.Chance(0.25f))
//                {
//                    CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
//                    if (calloutTracker != null)
//                    {
//                        calloutTracker.AttemptCallout(__instance.CasterPawn, CalloutDefOf.CM_Callouts_RulePack_Melee_Attack);
//                    }
//                }
//            }
//        }
//    }
//}

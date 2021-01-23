//using System;
//using System.Collections.Generic;

//using UnityEngine;
//using HarmonyLib;
//using RimWorld;
//using Verse;
//using Verse.AI;

//namespace CM_Callouts
//{
//    [StaticConstructorOnStartup]
//    public static class MoteThrown_Patches
//    {
//        [HarmonyPatch(typeof(MoteThrown))]
//        [HarmonyPatch("TimeInterval", MethodType.Normal)]
//        public static class MoteThrown_TimeInterval
//        {
//            [HarmonyPrefix]
//            public static void Prefix(MoteThrown __instance, ref float deltaTime, int ___lastMaintainTick)
//            {
//                if (!__instance.def.mote.realTime)
//                {
//                    deltaTime = (Find.TickManager.TicksGame - ___lastMaintainTick) / 60.0f;
//                }
//            }
//        }
//    }
//}

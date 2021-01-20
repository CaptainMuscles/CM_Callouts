﻿using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    [StaticConstructorOnStartup]
    public static class Pawn_DraftController_Patches
    {
        [HarmonyPatch(typeof(Pawn_DraftController))]
        [HarmonyPatch("Drafted", MethodType.Setter)]
        public static class Pawn_DraftController_Drafted
        {
            private static bool wereDrafted = false;

            [HarmonyPrefix]
            public static void Prefix(Pawn_DraftController __instance, bool value, bool ___draftedInt)
            {
                if (value && value != ___draftedInt)
                    wereDrafted = true;
            }

            [HarmonyPostfix]
            public static void Postfix(Pawn_DraftController __instance, bool value, bool ___draftedInt)
            {
                if (___draftedInt && wereDrafted && Rand.Chance(0.25f))
                {
                    CalloutUtility.AttemptDraftedCallout(__instance.pawn);
                }
            }
        }
    }
}

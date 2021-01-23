using System;
using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    public class PendingCallout
    {
        public Pawn initiator = null;
        public Pawn recipient = null;
        public PendingCallout(Pawn source, Pawn target)
        {
            initiator = source;
            recipient = target;
        }
    }
}

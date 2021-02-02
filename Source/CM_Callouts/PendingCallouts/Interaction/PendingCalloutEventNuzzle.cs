using System;
using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Interaction
{
    public class PendingCalloutEventNuzzle : PendingCalloutEvent
    {
        public Pawn initiator = null;
        public Pawn recipient = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventNuzzle(Pawn _initiator, Pawn _recipient)
        {
            initiator = _initiator;
            recipient = _recipient;
        }

        public override void AttemptCallout()
        {

        }
    }
}

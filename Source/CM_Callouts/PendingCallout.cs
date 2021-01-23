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
        public Pawn originalTarget = null;
        public ThingDef weaponDef = null;
        public ThingDef projectileDef = null;
        public ThingDef coverDef = null;

        public PendingCallout(Pawn _initiator, Pawn _recipient)
        {
            initiator = _initiator;
            recipient = _recipient;
        }

        public PendingCallout(Pawn _initiator, Pawn _recipient, Pawn _originalTarget, ThingDef _weaponDef, ThingDef _projectileDef, ThingDef _coverDef)
        {
            initiator = _initiator;
            recipient = _recipient;
            originalTarget = _originalTarget;

            weaponDef = _weaponDef;
            projectileDef = _projectileDef;
            coverDef = _coverDef;
        }
    }
}

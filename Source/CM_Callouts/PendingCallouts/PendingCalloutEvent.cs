using System;
using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Grammar;

namespace CM_Callouts
{
    public abstract class PendingCalloutEvent
    {
        public BodyDef body = null;
        public List<BodyPartRecord> bodyPartsDamaged = new List<BodyPartRecord>();
        public List<bool> bodyPartsDestroyed = new List<bool>();

        public abstract Thing Recipient { get; }

        public void FillBodyPartInfo(BodyDef _body, List<BodyPartRecord> _bodyPartsDamaged, List<bool> _bodyPartsDestroyed)
        {
            body = _body;
            bodyPartsDamaged = _bodyPartsDamaged;
            bodyPartsDestroyed = _bodyPartsDestroyed;
        }

        public abstract void AttemptCallout();
    }
}

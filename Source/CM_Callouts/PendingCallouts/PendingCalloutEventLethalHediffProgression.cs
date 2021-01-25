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
    public class PendingCalloutEventLethalHediffProgression : PendingCalloutEvent
    {
        public Pawn recipient = null;

        public Hediff hediff = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventLethalHediffProgression(Pawn _recipient, Hediff _hediff)
        {
            recipient = _recipient;
            hediff = _hediff;
        }

        public override void AttemptCallout()
        {
            CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
            if (calloutTracker != null)
            {
                //bool recipientCallout = calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Lethal_Hediff_Progression) && CalloutUtility.CanCalloutNow(recipient);
                bool recipientCallout = Rand.Chance(CalloutMod.settings.baseCalloutChance) && CalloutUtility.CanCalloutNow(recipient);

                if (recipientCallout)
                    DoRecipientCallout(calloutTracker);
            }
        }

        private void DoRecipientCallout(CalloutTracker calloutTracker)
        {
            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Lethal_Hediff_Progression;
            GrammarRequest grammarRequest = PrepareGrammarRequest(rulePack);
            calloutTracker.RequestCallout(recipient, rulePack, grammarRequest);
        }

        private GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
        {
            GrammarRequest grammarRequest = new GrammarRequest { Includes = { rulePack } };

            CalloutUtility.CollectPawnRules(recipient, "RECIPIENT", ref grammarRequest);
            CalloutUtility.CollectHediffRules(hediff, "CULPRITHEDIFF", ref grammarRequest);

            return grammarRequest;
        }
    }
}

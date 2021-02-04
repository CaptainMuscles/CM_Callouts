using System;
using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts.Combat
{
    public class PendingCalloutEventMeleeImpact : PendingCalloutEvent
    {
        public Pawn initiator = null;
        public Pawn recipient = null;

        public PendingCalloutEventMeleeImpact(Pawn _initiator, Pawn _recipient)
        {
            initiator = _initiator;
            recipient = _recipient;
        }

        public override void AttemptCallout()
        {
            base.AttemptCallout();

            CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
            if (calloutTracker != null)
            {
                bool initiatorCallout = Rand.Bool && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Melee_Attack_Landed) && CalloutUtility.CanCalloutNow(initiator);
                bool recipientCallout = Rand.Bool && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Melee_Attack_Received) && CalloutUtility.CanCalloutNow(recipient);

                if (initiatorCallout)
                    DoInitiatorCallout(calloutTracker);
                if (recipientCallout)
                    DoRecipientCallout(calloutTracker);
            }
        }

        private void DoInitiatorCallout(CalloutTracker calloutTracker)
        {
            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Melee_Attack_Landed;
            GrammarRequest grammarRequest = PrepareGrammarRequest(rulePack);
            calloutTracker.RequestCallout(initiator, rulePack, grammarRequest);
        }

        private void DoRecipientCallout(CalloutTracker calloutTracker)
        {
            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Melee_Attack_Received;
            GrammarRequest grammarRequest = PrepareGrammarRequest(rulePack);
            calloutTracker.RequestCallout(recipient, rulePack, grammarRequest);
        }

        private GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
        {
            GrammarRequest grammarRequest = new GrammarRequest { Includes = { rulePack } };

            CalloutUtility.CollectPawnRules(initiator, "INITIATOR", ref grammarRequest);

            if (recipient != null)
            {
                CalloutUtility.CollectPawnRules(recipient, "RECIPIENT", ref grammarRequest);
                grammarRequest.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("PART", body, bodyPartsDamaged, bodyPartsDestroyed, grammarRequest.Constants));
            }

            return grammarRequest;
        }
    }
}

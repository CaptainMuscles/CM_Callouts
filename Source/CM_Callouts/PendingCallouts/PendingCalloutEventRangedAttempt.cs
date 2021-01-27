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
    public class PendingCalloutEventRangedAttempt : PendingCalloutEvent
    {
        public Pawn initiator = null;
        public Pawn recipient = null;
        public Verb_LaunchProjectile verb = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventRangedAttempt(Pawn _initiator, Pawn _recipient, Verb_LaunchProjectile _verb)
        {
            initiator = _initiator;
            recipient = _recipient;
            verb = _verb;
        }

        public override void AttemptCallout()
        {
            CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
            if (calloutTracker != null)
            {
                bool initiatorCallout = Rand.Bool && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack) && CalloutUtility.CanCalloutNow(initiator);

                if (initiatorCallout)
                    DoInitiatorCallout(calloutTracker);
            }
        }

        private void DoInitiatorCallout(CalloutTracker calloutTracker)
        {
            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack;
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
                CalloutUtility.CollectCoverRules(recipient, initiator, "INITIATOR_COVER", verb, ref grammarRequest);

                CalloutUtility.CollectPawnRules(recipient, "RECIPIENT", ref grammarRequest);
                CalloutUtility.CollectCoverRules(initiator, recipient, "RECIPIENT_COVER", verb, ref grammarRequest);
            }

            return grammarRequest;
        }
    }
}

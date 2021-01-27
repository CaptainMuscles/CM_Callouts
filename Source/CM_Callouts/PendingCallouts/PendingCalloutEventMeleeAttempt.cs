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
    public class PendingCalloutEventMeleeAttempt : PendingCalloutEvent
    {
        public Pawn initiator = null;
        public Pawn recipient = null;
        public Verb verb = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventMeleeAttempt(Pawn _initiator, Pawn _recipient, Verb _verb)
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
                bool initiatorCallout = Rand.Bool && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Melee_Attack) && CalloutUtility.CanCalloutNow(initiator);

                if (initiatorCallout)
                    DoInitiatorCallout(calloutTracker);
            }
        }

        private void DoInitiatorCallout(CalloutTracker calloutTracker)
        {
            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Melee_Attack;
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

            if (verb != null && verb.maneuver != null && verb.maneuver.combatLogRulesHit != null)
                grammarRequest.Includes.Add(verb.maneuver.combatLogRulesHit);

            CalloutUtility.CollectPawnRules(initiator, "INITIATOR", ref grammarRequest);

            if (recipient != null)
                CalloutUtility.CollectPawnRules(recipient, "RECIPIENT", ref grammarRequest);

            return grammarRequest;
        }
    }
}

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
    public class PendingCalloutEventMeleeImpact : PendingCalloutEvent
    {
        public Pawn initiator = null;
        public Pawn recipient = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventMeleeImpact(Pawn _initiator, Pawn _recipient)
        {
            initiator = _initiator;
            recipient = _recipient;
        }

        public override void AttemptCallout()
        {
            CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
            if (calloutTracker != null)
            {
                bool initiatorCallout = Rand.Bool && Rand.Chance(CalloutMod.settings.baseCalloutChance) && calloutTracker.CanCalloutNow(initiator);
                bool recipientCallout = Rand.Bool && Rand.Chance(CalloutMod.settings.baseCalloutChance) && calloutTracker.CanCalloutNow(recipient);

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
                grammarRequest.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", body, bodyPartsDamaged, bodyPartsDestroyed, grammarRequest.Constants));
            }

            return grammarRequest;
        }
    }
}

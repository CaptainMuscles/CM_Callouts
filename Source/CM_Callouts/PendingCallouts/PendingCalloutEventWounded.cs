using System;
using System.Collections.Generic;

using UnityEngine;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Grammar;

namespace CM_Callouts.PendingCallouts
{
    public class PendingCalloutEventWounded : PendingCalloutEvent
    {
        public Pawn recipient = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventWounded(Pawn _recipient)
        {
            recipient = _recipient;
        }

        public override void AttemptCallout()
        {
            base.AttemptCallout();

            CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
            if (calloutTracker != null)
            {
                //bool recipientCallout = calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Wounded) && CalloutUtility.CanCalloutNow(recipient);
                bool recipientCallout = Rand.Chance(CalloutMod.settings.baseCalloutChance) && CalloutUtility.CanCalloutNow(recipient);

                if (recipientCallout)
                    DoRecipientCallout(calloutTracker);
            }
        }

        private void DoRecipientCallout(CalloutTracker calloutTracker)
        {
            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Wounded;
            GrammarRequest grammarRequest = PrepareGrammarRequest(rulePack);
            calloutTracker.RequestCallout(recipient, rulePack, grammarRequest);
        }

        private GrammarRequest PrepareGrammarRequest(RulePackDef rulePack)
        {
            GrammarRequest grammarRequest = new GrammarRequest { Includes = { rulePack } };

            CalloutUtility.CollectPawnRules(recipient, "RECIPIENT", ref grammarRequest);
            grammarRequest.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", body, bodyPartsDamaged, bodyPartsDestroyed, grammarRequest.Constants));

            return grammarRequest;
        }
    }
}

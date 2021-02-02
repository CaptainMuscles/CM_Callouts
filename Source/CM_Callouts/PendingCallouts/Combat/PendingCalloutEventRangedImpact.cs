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
    public class PendingCalloutEventRangedImpact : PendingCalloutEvent
    {
        public Pawn initiator = null;
        public Pawn recipient = null;
        public Pawn originalTarget = null;
        public ThingDef weaponDef = null;
        public ThingDef projectileDef = null;
        public ThingDef coverDef = null;

        public override Thing Recipient { get { return recipient; } }

        public PendingCalloutEventRangedImpact(Pawn _initiator, Pawn _recipient, Pawn _originalTarget, ThingDef _weaponDef, ThingDef _projectileDef, ThingDef _coverDef)
        {
            initiator = _initiator;
            recipient = _recipient;
            originalTarget = _originalTarget;

            weaponDef = _weaponDef;
            projectileDef = _projectileDef;
            coverDef = _coverDef;
        }

        public override void AttemptCallout()
        {
            CalloutTracker calloutTracker = Current.Game.World.GetComponent<CalloutTracker>();
            if (calloutTracker != null)
            {
                bool initiatorCallout = Rand.Bool && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack_Landed_OriginalTarget) && CalloutUtility.CanCalloutNow(initiator);
                bool recipientCallout = Rand.Bool && calloutTracker.CheckCalloutChance(CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack_Received_OriginalTarget) && CalloutUtility.CanCalloutNow(recipient);

                if (initiatorCallout)
                    DoInitiatorCallout(calloutTracker);
                if (recipientCallout)
                    DoRecipientCallout(calloutTracker);
            }
        }

        private void DoInitiatorCallout(CalloutTracker calloutTracker)
        {
            // Only if intended target was hit (for now)
            if (recipient != originalTarget)
                return;

            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack_Landed_OriginalTarget;
            GrammarRequest grammarRequest = PrepareGrammarRequest(rulePack);
            calloutTracker.RequestCallout(initiator, rulePack, grammarRequest);
        }

        private void DoRecipientCallout(CalloutTracker calloutTracker)
        {
            // Only if intended target was hit (for now)
            if (recipient != originalTarget)
                return;

            RulePackDef rulePack = CalloutDefOf.CM_Callouts_RulePack_Ranged_Attack_Received_OriginalTarget;
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

                if (coverDef != null)
                    grammarRequest.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT_COVER", coverDef));
            }

            return grammarRequest;
        }
    }
}

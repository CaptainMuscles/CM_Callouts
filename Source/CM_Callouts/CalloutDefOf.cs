using System.Collections.Generic;

using RimWorld;
using Verse;
using Verse.AI;

namespace CM_Callouts
{
    [DefOf]
    public static class CalloutDefOf
    {
        public static ThingDef CM_Callouts_Thing_Mote_Text_Ticked;
        public static ThingDef CM_Callouts_Thing_Mote_Text_Wound;
        public static ThingDef CM_Callouts_Thing_Mote_Attached_Text;

        public static RulePackDef CM_Callouts_RulePack_Ranged_Attack;

        public static RulePackDef CM_Callouts_RulePack_Ranged_Attack_Landed_OriginalTarget;
        public static RulePackDef CM_Callouts_RulePack_Ranged_Attack_Received_OriginalTarget;

        public static RulePackDef CM_Callouts_RulePack_Melee_Attack;

        public static RulePackDef CM_Callouts_RulePack_Melee_Attack_Landed;
        public static RulePackDef CM_Callouts_RulePack_Melee_Attack_Received;

        public static RulePackDef CM_Callouts_RulePack_Wounded;

        public static RulePackDef CM_Callouts_RulePack_Lethal_Hediff_Progression;

        public static RulePackDef CM_Callouts_RulePack_Drafted;
    }
}

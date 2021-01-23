using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CM_Callouts
{
    public class CalloutModSettings : ModSettings
    {
        public bool attachCalloutText = false;
        public bool showWounds = true;
        public float baseCalloutChance = 0.2f;

        public bool showDebugLogMessages = false;

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Values.Look(ref attachCalloutText, "attachCalloutText", true);
            Scribe_Values.Look(ref showWounds, "showWounds", false);
            Scribe_Values.Look(ref baseCalloutChance, "baseCalloutChance", 0.2f);
        }

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("CM_Callouts_Settings_Attach_Callout_Text_Label".Translate(), ref attachCalloutText, "CM_Callouts_Settings_Attach_Callout_Text_Description".Translate());
            listing_Standard.CheckboxLabeled("CM_Callouts_Settings_Show_Wounds_Label".Translate(), ref showWounds, "CM_Callouts_Settings_Show_Wounds_Description".Translate());

            listing_Standard.Label("CM_Callouts_Settings_Base_Callout_Chance_Label".Translate(), -1, "CM_Callouts_Settings_Base_Callout_Chance_Description".Translate());
            listing_Standard.Label(baseCalloutChance.ToString("P0"));
            baseCalloutChance = listing_Standard.Slider(baseCalloutChance, 0.0f, 1.0f);

            if (Prefs.DevMode)
            {
                listing_Standard.CheckboxLabeled("Show debug messages", ref showDebugLogMessages);
            }

            listing_Standard.End();
        }

        public void UpdateSettings()
        {
        }
    }
}

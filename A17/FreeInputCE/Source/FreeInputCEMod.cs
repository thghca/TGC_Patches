using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Verse;
using Harmony;

namespace FreeInputCE
{
    [StaticConstructorOnStartup]
    public static class FreeInputCEMod
    {
        static FreeInputCEMod()
        {
            Regex validNameRegex_free = new Regex(".*");

            
            try
            {
                Traverse.CreateWithType("RimWorld.Outfit").Field("ValidNameRegex").SetValue(validNameRegex_free);
            }
            catch (Exception e) { Log.Error(e.ToString()); }
            try
            {
                Traverse.CreateWithType("RimWorld.CharacterCardUtility").Field("validNameRegex").SetValue(validNameRegex_free);
            }
            catch (Exception e) { Log.Error(e.ToString()); }
            try
            {
                Traverse.CreateWithType("RimWorld.Dialog_ManageDrugPolicies").Field("ValidNameRegex").SetValue(validNameRegex_free);
            }
            catch (Exception e) { Log.Error(e.ToString()); }
            try
            {
                Traverse.CreateWithType("RimWorld.Dialog_ManageAreas").Field("ValidNameRegex").SetValue(validNameRegex_free);
            }
            catch (Exception e) { Log.Error(e.ToString()); }

            try
            {
                Traverse.CreateWithType("CombatExtended.Dialog_ManageLoadouts").Field("validNameRegex").SetValue(validNameRegex_free);
            }
            catch (Exception e) { Log.Message("FreeInputCE: Fix input for Extended loadout names skipped.\n" + e.ToString() ); }

            //HardcoreSK CoreSK.dll
            try
            {
                Traverse.CreateWithType("StorageSearch.Dialog_ManageOutfits_Patch").Field("ValidNameRegex").SetValue(validNameRegex_free);
            }
            catch (Exception e) { Log.Message("FreeInputCE: Fix input for HardcoreSK outfit names skipped.\n" + e.ToString()); }
        }
    }
}

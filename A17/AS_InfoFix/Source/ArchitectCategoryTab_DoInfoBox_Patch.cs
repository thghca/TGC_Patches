using Harmony;
using Verse;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace AS_InfoFix
{
    [HarmonyPatch(typeof(ArchitectCategoryTab))]
    [HarmonyPatch("DoInfoBox")]
    public static class ArchitectCategoryTab_DoInfoBox_Patch
    {

        public static bool Prefix(ArchitectCategoryTab __instance, Rect infoRect, ref Designator designator)
        {
            var current = Main.currentMouseOver;
            if (current != null && current != designator)
            {
                designator = current;
            }            
            return true;
        }
    }

}

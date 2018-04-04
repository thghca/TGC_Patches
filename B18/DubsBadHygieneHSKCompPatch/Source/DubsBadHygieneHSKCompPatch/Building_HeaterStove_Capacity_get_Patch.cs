using DubsBadHygiene;
using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DubsBadHygieneHSKCompPatch
{
    [HarmonyPatch(typeof(DubsBadHygiene.Building_HeaterStove))]
    [HarmonyPatch("Capacity",PropertyMethod.Getter)]
    public static class Building_HeaterStove_Capacity_get_Patch
    {
        public static bool Prefix(Building_HeaterStove __instance, ref float __result)
        {
            __result = Main.Powered(__instance) ? 1000 : 0;
            return false;
        }
    }
}

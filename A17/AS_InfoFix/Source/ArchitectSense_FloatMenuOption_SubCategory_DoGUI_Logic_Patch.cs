using Harmony;
using System.Reflection;
using UnityEngine;

namespace AS_InfoFix
{
    [HarmonyPatch]
    public static class FloatMenu_SubCategory_DoWindowContents_Patch
    {

        public static MethodInfo TargetMethod()
        {
            return AccessTools.TypeByName("ArchitectSense.FloatMenu_SubCategory").GetMethod("DoWindowContents");
        }

        public static void Prefix()
        {
            Main.mouseOver = false;
        }

        public static void Postfix()
        {
            if (!Main.mouseOver) Main.currentMouseOver = null;
        }
    }
}

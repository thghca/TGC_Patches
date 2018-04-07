using Harmony;
using System.Reflection;
using RimWorld;

namespace AS_InfoFix
{
    [HarmonyPatch]
    public static class FloatMenuOption_SubCategory_DoGUI_BG_Patch
    {


        public static MethodInfo TargetMethod()
        {
            return AccessTools.TypeByName("ArchitectSense.FloatMenuOption_SubCategory").GetMethod("DoGUI_BG");
        }

        public static void Postfix(object __instance, bool __result)
        {
            if (__result)
            {
                Main.mouseOver = true;
                Main.currentMouseOver = Traverse.Create(__instance).Field("gizmo").GetValue() as Designator_Build;
            }
        }
    }

}

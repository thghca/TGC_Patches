using Harmony;
using Verse;
using System;

namespace AS_InfoFix
{
    [HarmonyPatch(typeof(WindowStack))]
    [HarmonyPatch("TryRemove")]
    [HarmonyPatch(new Type[]{typeof(Window), typeof(bool)})]
    public static class WindowStack_TryRemove_Patch
    {

        public static void Postfix(Window window, bool doCloseSound = true)
        {
            if(window.GetType().IsAssignableFrom(AccessTools.TypeByName("ArchitectSense.FloatMenu_SubCategory")))
            Main.currentMouseOver = null;
        }
    }

}

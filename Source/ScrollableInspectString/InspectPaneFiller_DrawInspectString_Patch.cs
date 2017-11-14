using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace ScrollableInspectString
{
    [StaticConstructorOnStartup]
    [HarmonyPatch]
    public static class InspectPaneFiller_DrawInspectString_Patch
    {
        private static Vector2 scrollbarPosition;

        static InspectPaneFiller_DrawInspectString_Patch()
        {
            var harmony = HarmonyInstance.Create("thghca.ScrollableInspectString");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("ScrollableInspectString");
        }

        static MethodInfo TargetMethod() => AccessTools.TypeByName("RimWorld.InspectPaneFiller").GetMethod("DrawInspectString");
       
        public static bool Prefix(string str, ref float y)
        {
            Text.Font = GameFont.Small;        
            Rect rect = new Rect(0f, y, InspectPaneUtility.PaneInnerSize.x, InspectPaneUtility.PaneInnerSize.y-y);
            Widgets.LabelScrollable(rect, str, ref scrollbarPosition);
            return false;
        }
    }
}

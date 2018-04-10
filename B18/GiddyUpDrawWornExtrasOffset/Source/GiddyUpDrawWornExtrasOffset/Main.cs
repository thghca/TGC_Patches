using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace GiddyUpDrawWornExtrasOffset
{
    public class Main : Mod
    {
        //TODO: GiddyUpCore/Source/Giddy-up-Core/Harmony/Pawn_DrawAt.cs addCustomOffsets
        public static string logPrefix = "GU offset: ";

        public Main(ModContentPack content) : base(content)
        {
            var harmony = HarmonyInstance.Create("thghca.GiddyUpDrawWornExtrasOffset");
            Log.Message(Main.logPrefix + "init");
            try
            {
                PatchDrawPos(harmony);
                PatchCombatExtended(harmony);
                if(AccessTools.TypeByName("WHands.HandDrawer") != null) PatchWhands(harmony);
            }
            catch (Exception e)
            {
                Log.Error(Main.logPrefix+"init failed:\n" + e.ToString());
            }
        }

        private static void PatchDrawPos(HarmonyInstance harmony)
        {
            harmony.Patch(
                typeof(Verse.Pawn_DrawTracker).GetProperty("DrawPos").GetGetMethod(),
                null,
                new HarmonyMethod(typeof(DrawPos_Patch).GetMethod("Postfix"))
                );
            Log.Message(Main.logPrefix + "DrawPos Patched");
        }

        private static void PatchCombatExtended(HarmonyInstance harmony)
        {
            //CombatExtended
            string[] patchlist = { "CombatExtended.Apparel_VisibleAccessory", "CombatExtended.Apparel_Shield" };
            foreach (var s in patchlist)
            {
                Type type = AccessTools.TypeByName(s);
                if (type == null) continue;
                harmony.Patch(
                    type.GetMethod("DrawWornExtras"),
                    new HarmonyMethod(typeof(DrawWornExtras_Patch).GetMethod("Prefix")),
                    new HarmonyMethod(typeof(DrawWornExtras_Patch).GetMethod("Postfix"))
                    );
                Log.Message(Main.logPrefix + type.Name + " Patched");
            }
        }

        private static void PatchWhands(HarmonyInstance harmony)
        {
                harmony.Patch(
                               AccessTools.TypeByName("WHands.HandDrawer").GetMethod("PostDraw"),
                               new HarmonyMethod(typeof(Whands_HandDrawer_PostDraw_Patch).GetMethod("Prefix")),
                               new HarmonyMethod(typeof(Whands_HandDrawer_PostDraw_Patch).GetMethod("Postfix"))
                               );
                Log.Message(Main.logPrefix + "Whands Patched");
        }
    }
}

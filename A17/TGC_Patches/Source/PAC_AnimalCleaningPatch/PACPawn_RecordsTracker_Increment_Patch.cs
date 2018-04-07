using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace PAC_AnimalCleaningPatch
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(PawnsAreCapable.HarmonyPatches.Pawn_RecordsTracker_Increment))]
    [HarmonyPatch("Prefix")]
    public static class PACPawn_RecordsTracker_Increment_Patch
    {
        static PACPawn_RecordsTracker_Increment_Patch()
        {
            var harmony = HarmonyInstance.Create("thghca.PAC_AnimalCleaningPatch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("PAC_AnimalCleaningPatch");
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            //if not valid return;
            yield return new CodeInstruction(OpCodes.Ldarg_1);
            yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PawnsAreCapable.HarmonyPatches.Pawn_RecordsTracker_Increment), "Pawn"));
            yield return new CodeInstruction(OpCodes.Call, typeof(PACPawn_RecordsTracker_Increment_Patch).GetMethod(nameof(ValidPawn)));
            Label label = new Label();
            yield return new CodeInstruction(OpCodes.Brtrue_S,label);
            yield return new CodeInstruction(OpCodes.Ret);
            yield return new CodeInstruction(OpCodes.Nop,label);
            foreach (CodeInstruction instruction in instructions) yield return instruction;
        }

        public static bool ValidPawn(Pawn pawn)
        {
            if (pawn?.story?.traits == null || pawn?.needs?.mood?.thoughts?.memories == null) return false;
            return true;
        }
    }
}

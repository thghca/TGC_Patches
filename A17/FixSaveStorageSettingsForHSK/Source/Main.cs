using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace FixSaveStorageSettingsForHSK
{

    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = HarmonyInstance.Create("net.thghca.rimworld.mod.FixSaveStorageSettingsForHSK");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    //Get
    [HarmonyPatch(typeof(Dialog_ManageOutfits))]
    [HarmonyPatch("get_SelectedOutfit")]
    public static class Dialog_ManageOutfits_GetSelectedOutfit_Patch
    {
        static void Postfix(Dialog_ManageOutfits __instance, ref Outfit __result)
        {
            __result = (Outfit)AccessTools.TypeByName("StorageSearch.Dialog_ManageOutfits_Patch")
                .GetProperty("SelectedOutfit", AccessTools.all).GetValue(null, null);
        }

    }
    //Set
    [HarmonyPatch(typeof(Dialog_ManageOutfits))]
    [HarmonyPatch("set_SelectedOutfit")]
    public static class Dialog_ManageOutfits_SetSelectedOutfit_Patch
    {
        static void Postfix(Outfit value)
        {
            AccessTools.TypeByName("StorageSearch.Dialog_ManageOutfits_Patch")
                .GetProperty("SelectedOutfit", AccessTools.all).SetValue(null, value, null);
        }
    }


    [HarmonyPatch]
    [HarmonyBefore(new string[] { "com.savestoragesettings.rimworld.mod" })]
    public static class Patch_Dialog_ManageOutfits_DoWindowContents
    {

        static MethodInfo TargetMethod()
        {
            return AccessTools.TypeByName("SaveStorageSettings.Patch_Dialog_ManageOutfits_DoWindowContents").GetMethod("Postfix",AccessTools.all);
        }
        //IL_0073: ldc.r4    220 =>320
        //IL_0078: ldc.r4    49
        //IL_007D: ldc.r4    300
        //IL_0082: ldc.r4    32
        //IL_0087: newobj    instance void [UnityEngine]UnityEngine.Rect::.ctor(float32, float32, float32, float32)
        [HarmonyAfter(new string[] { "com.savestoragesettings.rimworld.mod" })]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
 
            bool found = false;
            int startIndex = -1;
            int endIndex = -1;

            var list = instructions.ToList();
            var cnt = list.Count;

            for (int i = 0; i < cnt; ++i)
            {
               
                var instruction = list[i];
                if (!found)
                {

                    if (instruction.opcode == OpCodes.Newobj)
                    {
                        found = true;
                        endIndex = i;
                        startIndex = i - 4;

                        bool f = false;
                        try
                        {
                            f = Convert.ToSingle(list[i - 4].operand).Equals(220f);
                        }
                        catch { }
                        if (!f) found = false;
                    }
                }
            }

            if (found)
            {
                list[startIndex] = new CodeInstruction(list[startIndex].opcode, 320f);
            }
            return list;
        }
    }
}

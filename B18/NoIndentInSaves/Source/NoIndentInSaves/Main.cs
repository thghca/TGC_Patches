using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace NoIndentInSaves
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            //HarmonyInstance.DEBUG = true;
            var harmony = HarmonyInstance.Create("thghca.NoIndentInSaves");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("NoIndentInSaves");
        }
    }


    [HarmonyPatch(typeof(Verse.ScribeSaver))]
    [HarmonyPatch("InitSaving")]
    public static class Building_Heater_Draw_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; ++i)
            {
                if (codes[i].opcode == OpCodes.Callvirt && (MethodInfo)codes[i].operand == typeof(System.Xml.XmlWriterSettings).GetMethod("set_Indent"))
                {
                    if (codes[i - 1].opcode == OpCodes.Ldc_I4_1)
                    {
                        codes[i - 1] = new CodeInstruction(codes[i - 1]) {opcode = OpCodes.Ldc_I4_0 };
                        break;
                    }
                }
            }
            return codes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Harmony;
using RimWorld;
using System.Reflection;
using System.Reflection.Emit;

namespace CustomMapSize
{
    [StaticConstructorOnStartup]
    public class CustomMapSizeMod : Mod
    {     
        static CustomMapSizeMod()
        {
            Log.Message("CustomMapSize Init");
            var harmony = HarmonyInstance.Create("thghca.CustomMapSize");      
            harmony.Patch(AccessTools.Method(typeof(Dialog_AdvancedGameConfig), "DoWindowContents"), transpiler: new HarmonyMethod(type: typeof(CustomMapSizeMod), name: nameof(Transpiler)));
        }

        public CustomMapSizeMod(ModContentPack content) : base(content)
        {
        }

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            MethodInfo listing_NewColumn = AccessTools.Method(type: typeof(Listing), name: nameof(Listing.NewColumn));
            MethodInfo listing_Standard_Label = AccessTools.Method(type: typeof(Listing_Standard), name: nameof(Listing_Standard.Label));
            int column = 1;
            int label = 0;
            bool patched1 = false;
            bool patched3 = false;
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.operand == listing_NewColumn)
                {
                    if (column == 1 && !patched1)
                    {
                        yield return new CodeInstruction(opcode: OpCodes.Call, operand: AccessTools.Method(typeof(CustomMapSizeMod), "DoCustomMapSize"));
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_0);
                        patched1 = true;
                    }
                    column++;
                    yield return instruction;
                    continue;
                }
                if (column == 3 && !patched3)
                {
                    if (instruction.operand == listing_Standard_Label)
                    {
                        label++;
                        yield return instruction;
                        continue;
                    }
                    if (label == 2)
                    {
                        yield return new CodeInstruction(opcode: OpCodes.Ldloc_0);
                        yield return new CodeInstruction(opcode: OpCodes.Call, operand: AccessTools.Method(typeof(CustomMapSizeMod), "DoCustomMapSizeWarning"));
                        patched3 = true;
                        yield return instruction;
                        continue;
                    }
                }
                yield return instruction;
            }
        }

        static int customSize = 450;
        static string customSizeBuf;
        static bool flag=false;
        static int min = 1;
        static int max = 5000;
        static int sizeTooSmall = 25;
        static int sizeTooLarge = 1000;
        public static void DoCustomMapSize(Listing_Standard listing_Standard)
        {
            listing_Standard.Gap(10f);
            listing_Standard.CheckboxLabeled("CustomMapSize.Label".Translate(), ref flag);
            if (flag)
            {
                Find.GameInitData.mapSize = customSize;
            }         
            listing_Standard.TextFieldNumericLabeled<int>(
                "CustomMapSize.FieldLabel".Translate(min,max), ref customSize, ref customSizeBuf, min, max);
            string desc = "MapSizeDesc".Translate(customSize, customSize * customSize);
            listing_Standard.Label(desc, -1f, null);

        }
        public static void DoCustomMapSizeWarning(Listing_Standard listing_Standard)
        {
            if (flag)
            {
                listing_Standard.Label("CustomMapSize.Warning".Translate(), -1f, null);
            }
            if (Find.GameInitData.mapSize < sizeTooSmall)
            {
                listing_Standard.Label("CustomMapSize.WarningTooSmall".Translate(), -1f, null);
            }
            if (Find.GameInitData.mapSize > sizeTooLarge)
            {
                listing_Standard.Label("CustomMapSize.WarningTooLarge".Translate(), -1f, null);
            }
        }
    }
}

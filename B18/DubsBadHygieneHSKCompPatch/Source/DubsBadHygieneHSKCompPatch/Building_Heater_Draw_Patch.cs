using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace DubsBadHygieneHSKCompPatch
{
    [HarmonyPatch(typeof(DubsBadHygiene.Building_HeaterStove))]
    [HarmonyPatch("Draw")]
    public static class Building_Heater_Draw_Patch
    {
        //if (this.fuelComp != null && this.fuelComp.HasFuel... => true && Powered(this)

        //IL_0006: ldarg.0
        //IL_0007: ldfld class ['Assembly-CSharp'] RimWorld.CompRefuelable DubsBadHygiene.Building_Heater::fuelComp => NOP
        //IL_000C: brfalse.s IL_008D

        //IL_000E: ldarg.0
        //IL_000F: ldfld     class ['Assembly-CSharp'] RimWorld.CompRefuelable DubsBadHygiene.Building_Heater::fuelComp => NOP
        //IL_0014: callvirt instance bool['Assembly-CSharp'] RimWorld.CompRefuelable::get_HasFuel()  call Powered
        //IL_0019: brfalse.s IL_008D


        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; ++i)
            {
                if (codes[i].opcode == OpCodes.Ldfld && (FieldInfo)codes[i].operand == typeof(DubsBadHygiene.Building_HeaterStove).GetField(nameof(DubsBadHygiene.Building_HeaterStove.fuelComp)))
                {
                    //this already on stack and it is not null (true), so just skip load field
                    yield return new CodeInstruction(OpCodes.Nop);
                    continue;
                }
                if (codes[i].opcode == OpCodes.Callvirt && (MethodInfo)codes[i].operand == typeof(CompRefuelable).GetProperty(nameof(CompRefuelable.HasFuel)).GetGetMethod())
                {
                    yield return new CodeInstruction(OpCodes.Call, typeof(Main).GetMethod(nameof(Main.Powered)));
                    continue;
                }
                yield return codes[i];
            }
        }
    }
}

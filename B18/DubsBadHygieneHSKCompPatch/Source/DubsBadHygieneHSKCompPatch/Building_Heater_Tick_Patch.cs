//using Harmony;
//using RimWorld;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Reflection.Emit;
//using System.Text;
//using Verse;

//namespace DubsBadHygieneHSKCompPatch
//{

//    [HarmonyPatch(typeof(DubsBadHygiene.Building_HeaterStove))]
//    [HarmonyPatch("Tick")]
//    public static class Building_Heater_Tick_Patch
//    {


//        //if (this.fuelComp != null && this.fuelComp.HasFuel) => if(true && Powered(this))

//        //if (this.fuelComp == null && this.powerComp == null) => if(false && this.powerComp == null)


//        //IL_0092: ldarg.0
//        //IL_0093: ldfld class ['Assembly-CSharp'] RimWorld.CompRefuelable DubsBadHygiene.Building_Heater::fuelComp   => NOP
//        //IL_0098: brfalse.s IL_00B0
//        //IL_009A: ldarg.0
//        //IL_009B: ldfld     class ['Assembly-CSharp'] RimWorld.CompRefuelable DubsBadHygiene.Building_Heater::fuelComp => NOP
//        //IL_00A0: callvirt instance bool['Assembly-CSharp'] RimWorld.CompRefuelable::get_HasFuel()  => call Powered
//        //IL_00A5: brfalse.s IL_00B0

//        //IL_00B0: ldarg.0
//        //IL_00B1: ldfld class ['Assembly-CSharp'] RimWorld.CompRefuelable DubsBadHygiene.Building_Heater::fuelComp  =>NOP
//        //IL_00B6: brtrue.s IL_00C7 
//        //IL_00B8: ldarg.0
//        //IL_00B9: ldfld     class ['Assembly-CSharp'] RimWorld.CompPowerTrader DubsBadHygiene.Building_Heater::powerComp
//        //IL_00BE: brtrue.s IL_00C7


//        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
//        {
//            var codes = new List<CodeInstruction>(instructions);
//            for (int i = 0; i < codes.Count; ++i)
//            {
//                if (codes[i].opcode == OpCodes.Ldfld && (FieldInfo)codes[i].operand == typeof(DubsBadHygiene.Building_HeaterStove).GetField(nameof(DubsBadHygiene.Building_HeaterStove.fuelComp)))
//                {
//                    //this already on stack and it is not null (true), so just skip load field
//                    yield return new CodeInstruction(OpCodes.Nop);
//                    continue;
//                }
//                if(codes[i].opcode==OpCodes.Callvirt && (MethodInfo)codes[i].operand == typeof(CompRefuelable).GetProperty(nameof(CompRefuelable.HasFuel)).GetGetMethod())
//                {
//                    yield return new CodeInstruction(OpCodes.Call, typeof(Main).GetMethod(nameof(Main.Powered)));
//                    continue;
//                }
//                yield return codes[i];
//            }
//        }
//    }
//}

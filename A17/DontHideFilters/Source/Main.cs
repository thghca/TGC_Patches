using Verse;
using Harmony;
using UnityEngine;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace DontHideFilters
{
    
    [StaticConstructorOnStartup]
    class Main
    {
        static Main()
        {
            //FileLog.Reset();
            var harmony = HarmonyInstance.Create("net.thghca.rimworld.mod.donthidefilters");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Listing_TreeThingFilter))]
    [HarmonyPatch("CalculateHiddenSpecialFilters")]
    public static class Listing_TreeThingFilter_CalculateHiddenSpecialFilters_Patch
    {
        /*
        replace this.filter.DisplayRootCategory.catDef
        with ThingCategoryNodeDatabase.RootNode
        *in
        IEnumerable<ThingDef> enumerable2 = this.filter.DisplayRootCategory.catDef.DescendantThingDefs;
	    IL_0057: ldarg.0
	    IL_0058: ldfld class Verse.ThingFilter Verse.Listing_TreeThingFilter::'filter'
	    IL_005d: callvirt instance class Verse.TreeNode_ThingCategory Verse.ThingFilter::get_DisplayRootCategory()
	    IL_0062: ldfld class Verse.ThingCategoryDef Verse.TreeNode_ThingCategory::catDef
	    IL_0067: callvirt instance class [mscorlib]System.Collections.Generic.IEnumerable`1<class Verse.ThingDef> Verse.ThingCategoryDef::get_DescendantThingDefs()
	    IL_006c: stloc.1
        */
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
                 //FileLog.Log(i.ToString() +": "+ instruction.ToString());
                //if (instruction.operand != null) FileLog.Log("---" + instruction.operand.ToString());
                if (!found)
                {
                    if (instruction.opcode == OpCodes.Callvirt && instruction.operand == AccessTools.Property(typeof(Verse.ThingCategoryDef), "DescendantThingDefs").GetGetMethod())
                    {
                        //FileLog.Log("!END " + i);
                        endIndex = i;

                        for (var j = endIndex; j > 0; --j)
                        {
                            if (list[j].opcode == OpCodes.Ldarg_0)
                            {
                                startIndex = j;
                                //FileLog.Log("!START " + j);
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }

            if(found)
            {
                for (int i = startIndex; i < endIndex; ++i)
                    if(list[i].opcode==OpCodes.Callvirt && list[i].operand == AccessTools.Property(typeof(Verse.ThingFilter), "DisplayRootCategory").GetGetMethod())
                    {
                       // FileLog.Log("!REPLACE " + i);
                        list[i] = new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(Verse.ThingCategoryNodeDatabase), "RootNode").GetGetMethod());
                        //FileLog.Log(i.ToString() + ": " + list[i].ToString());
                        //if (list[i].operand != null) FileLog.Log("---" + list[i].operand.ToString());
                        for (int j = startIndex; j < i; ++j) 
                        {
                            list[j] = new CodeInstruction(OpCodes.Nop);
                            //FileLog.Log("!NOP " + j);
                            //FileLog.Log(j.ToString() + ": " + list[j].ToString());
                            if (list[j].operand != null) FileLog.Log("---" + list[j].operand.ToString());
                        }
                    }
            }
            return list;
        }    
    }
}

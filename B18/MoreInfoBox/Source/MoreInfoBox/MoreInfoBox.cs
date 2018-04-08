using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace MoreInfoBox
{
    public class MoreInfoBox:Mod
    {
        public static bool fertileFields = false;
        public static MethodInfo miGetModExtension;

        public MoreInfoBox(ModContentPack content) : base(content)
        {
            Log.Message("MoreInfoBox init");
            Harmony.HarmonyInstance.Create("thghca.MoreInfoBox").PatchAll(Assembly.GetExecutingAssembly());
            if(AccessTools.TypeByName("FertileFields.Terrain") != null)
            {
                fertileFields = true;
                miGetModExtension = 
                    AccessTools.Method(
                        typeof(Def), 
                        nameof(Def.GetModExtension), 
                        new Type[] { }, new Type[] { AccessTools.TypeByName("FertileFields.Terrain")}
                    );
                Log.Message("MoreInfoBox FertileFields found");
            }


            
        }
    }

    [HarmonyPatch(typeof(Designator_Build))]
    [HarmonyPatch("Desc",PropertyMethod.Getter)]
    public static class Designator_Build_Desc_Get_Patch
    {
        
        public static void Postfix(Designator_Build __instance, ref string __result)
        {
            try
            {
                BuildableDef entDef = Traverse.Create(__instance)?.Field("entDef")?.GetValue<BuildableDef>();
                ThingDef stuffDef = Traverse.Create(__instance)?.Field("stuffDef")?.GetValue<ThingDef>();

                string walkspeed = "";
                string fertility = "";
                string beauty = "";
                string cleanliness = "";

                //FertileFields terraforming
                if(MoreInfoBox.fertileFields) entDef = TryGetFertileFieldsTerrainDef(entDef);

                if (entDef is TerrainDef)
                {
                    if (entDef.passability != Traversability.Impassable)
                        walkspeed = "MIB.Speed".Translate(SpeedPercentString(entDef.pathCost));
                    else
                        walkspeed = "MIB.Impassable".Translate();

                    if (entDef.fertility > 0)
                        fertility =
                            "MIB.Fertility".Translate(entDef.fertility.ToStringPercent());
                }

                if (entDef.HasStat(StatDefOf.Beauty))
                    beauty = "MIB.Beauty".Translate(entDef.GetStat(stuffDef, StatDefOf.Beauty));

                if (entDef.HasStat(StatDefOf.Cleanliness))
                {
                    cleanliness = "MIB.Cleanliness".Translate(entDef.GetStat(stuffDef, StatDefOf.Cleanliness).ToStringByStyle(ToStringStyle.Integer));
                }

                //extra linebreak
                bool extraLineBreak = Text.Font == GameFont.Tiny;
                if (extraLineBreak)
                {
                    var tmp = Text.Font;
                    Text.Font = GameFont.Small;
                    extraLineBreak = Text.CalcHeight(__instance.LabelCap, ArchitectCategoryTab.InfoRect.width - 7 * 2) > 25;
                    Text.Font = tmp;
                }

                __result = (extraLineBreak ? "\n" : "") + walkspeed + fertility + beauty + cleanliness + "\n" + __result;
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        private static bool HasStat(this BuildableDef entDef, StatDef statDef)
        {
            return entDef?.statBases?.StatListContains(statDef) ?? false;
        }

        private static float GetStat(this BuildableDef entDef, ThingDef stuffDef, StatDef statDef)
        {
            if (stuffDef != null)
                return entDef.GetStatValueAbstract(StatDefOf.Beauty, stuffDef);
            else
                return entDef.GetStatValueAbstract(StatDefOf.Beauty);
        }

        
        private static BuildableDef TryGetFertileFieldsTerrainDef(BuildableDef bDef)
        {
            object modext = MoreInfoBox.miGetModExtension.Invoke(bDef, null);
            if(modext == null) return bDef;
            return Traverse.Create(modext).Field("result").GetValue<BuildableDef>();
        }

        private static string SpeedPercentString(float extraPathTicks)
        {
            float f = 13f / (extraPathTicks + 13f);
            return f.ToStringPercent();
        }
    }
}

using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace DubsBadHygieneHSKCompPatch
{
    [StaticConstructorOnStartup]
    public static class Main
    {
        static Main()
        {
            //HarmonyInstance.DEBUG = true;
            var harmony = HarmonyInstance.Create("thghca.DubsBadHygieneHSKCompPatch");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Log.Message("DubsBadHygieneHSKCompPatch");
        }

        public static bool Powered(DubsBadHygiene.Building_Heater heater)
        {
            var fueledComp = heater.GetComp<SK.CompFueled>();
            var breakdownableComp = heater.GetComp<CompBreakdownable>();
            return (
                fueledComp != null //&& fueledComp.fuel.count >= 0
                && fueledComp.internalTemp > fueledComp.Props.operatingTemp
                //&& fueledComp.compFlickable != null && fueledComp.compFlickable.SwitchIsOn
                && breakdownableComp != null && !breakdownableComp.BrokenDown);
        }
    }
}

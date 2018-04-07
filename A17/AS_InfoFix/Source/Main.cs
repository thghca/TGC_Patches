using Harmony;
using Verse;
using System.Reflection;
using RimWorld;

namespace AS_InfoFix
{
    [StaticConstructorOnStartup]
    public class Main
    {
        public static Designator_Build currentMouseOver;
        public static bool mouseOver;

        static Main()
            {
                var harmony = HarmonyInstance.Create("net.thghca.rimworld.mod.AS_InfoFix");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
    }

}

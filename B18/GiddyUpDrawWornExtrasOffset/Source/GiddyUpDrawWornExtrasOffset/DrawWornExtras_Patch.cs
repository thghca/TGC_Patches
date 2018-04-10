using GiddyUpCore;
using GiddyUpCore.Storage;
using RimWorld;
using UnityEngine;

namespace GiddyUpDrawWornExtrasOffset
{
    public static class DrawWornExtras_Patch
    {
        public static void Prefix(Apparel __instance)
        {
            if (__instance.Wearer == null || !__instance.Wearer.Spawned)
            {
                return;
            }

            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance.Wearer);
            if (pawnData.mount == null)
            {
                return;
            }
            
            DrawPos_Patch.offset = new Vector3(0, 0, pawnData.drawOffset);
            DrawPos_Patch.offsetEnabled = true;
        }

        public static void Postfix()
        {
            DrawPos_Patch.offsetEnabled = false;
        }
    }
}

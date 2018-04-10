using GiddyUpCore;
using GiddyUpCore.Storage;
using UnityEngine;
using Verse;

namespace GiddyUpDrawWornExtrasOffset
{
    public static class Whands_HandDrawer_PostDraw_Patch
    {
        public static void Prefix(ThingComp __instance)
        {
            ExtendedPawnData pawnData = Base.Instance.GetExtendedDataStorage().GetExtendedDataFor(__instance.parent as Pawn);
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

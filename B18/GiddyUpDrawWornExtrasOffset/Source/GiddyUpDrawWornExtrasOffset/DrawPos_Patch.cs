using UnityEngine;

namespace GiddyUpDrawWornExtrasOffset
{
    public static class DrawPos_Patch
    {
        public static bool offsetEnabled = false;
        public static Vector3 offset = Vector3.zero;

        public static void Postfix(ref Vector3 __result)
        {
            if (offsetEnabled)
            {
                __result += offset;
            }
        }
    }
}

using Harmony;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Verse;

namespace SKLangFallback
{
    [StaticConstructorOnStartup]
    public class SKLangFallbackMod:Mod
    {
        public static string LangPostfix = "-SK";
        public SKLangFallbackMod(ModContentPack content):base(content)
        {
            Log.Message("SKLangFallback init");
            Harmony.HarmonyInstance.Create("thghca.SKLangFallback").PatchAll(Assembly.GetExecutingAssembly());

        }
    }

    [HarmonyPatch(typeof(Verse.LoadedLanguage))]
    [HarmonyPatch("FolderPaths",PropertyMethod.Getter)]
    public static class LoadedLanguage_Patch
    {
        public static bool Prefix(Verse.LoadedLanguage __instance, ref IEnumerable<string> __result)
        {
            if (!__instance.folderName.EndsWith(SKLangFallbackMod.LangPostfix))
                return true;
            __result = FolderPaths(__instance);
            return false;
        }

        public static IEnumerable<string> FolderPaths(Verse.LoadedLanguage lang)
        {
            foreach (ModContentPack mod in LoadedModManager.RunningMods)
            {
                string langDirPath = Path.Combine(mod.RootDir, "Languages");
                string myDirPath = Path.Combine(langDirPath, lang.folderName);
                DirectoryInfo myDir = new DirectoryInfo(myDirPath);
                if (myDir.Exists)
                {
                    yield return myDirPath;
                    continue;
                }
                //---
                myDirPath = myDirPath.Remove(myDirPath.Length - SKLangFallbackMod.LangPostfix.Length);
                myDir = new DirectoryInfo(myDirPath);
                //Log.Warning(myDirPath);
                if (myDir.Exists)
                {
                    Log.Warning("Translation "+ mod.Name+" fallback to "+myDirPath);
                    yield return myDirPath;
                    continue;
                }
                //Log.Warning("Translation " + mod.Name + "not found");
                //---
            }
            yield break;
        }
    }
}


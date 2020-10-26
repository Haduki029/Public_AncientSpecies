using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using HarmonyLib;

namespace AS_WHE.HarmonyPatch
{
    public static class ASWHEPatch
    {
        private static readonly Type patchType = typeof(ASWHEPatch);
        static ASWHEPatch()
        {
            Harmony harmonyInstance = new Harmony("rimworld.as.whe.mod");
            harmonyInstance.Patch(AccessTools.Method(typeof(IncidentWorker_WildManWandersIn), "TryExecuteWorker", null, null), new HarmonyMethod(patchType, "TryExecuteWorkerPrefix", null), null, null);
        }

    }
}

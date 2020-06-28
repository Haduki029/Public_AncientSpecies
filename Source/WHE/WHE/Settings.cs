using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;

namespace AS_WHE
{
    [StaticConstructorOnStartup]
    class PatchSetClass
    {
        static PatchSetClass()
        {
            Harmony harmonyInstance = new Harmony("com.ColorPatch.rimworld.mod");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PawnApparelGenerator))]
    [HarmonyPatch("GenerateStartingApparelFor")]
    public static class PawnGenColorClass
    {
        [HarmonyPostfix]
        private static void Postfix(ref Pawn pawn)
        {
            if (pawn.apparel != null)
            {
                List<Apparel> wornApparel = pawn.apparel.WornApparel;
                for (int i = 0; i < wornApparel.Count; i++)
                {
                    CustomThingDef colorDef = wornApparel[i].def as CustomThingDef;
                    if (colorDef != null && !colorDef.followStuffColor)
                    {
                        CompColorableUtility.SetColor(wornApparel[i], Color.white, true);
                        CompColorableUtility.SetColor(wornApparel[i], Color.black, true);
                        CompColorableUtility.SetColor(wornApparel[i], Color.white, true);
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(ThingMaker))]
    [HarmonyPatch("MakeThing")]
    public static class ThingMakeColorClass
    {
        [HarmonyPostfix]
        static void Postfix(ref Thing __result)
        {
            ThingWithComps twc = __result as ThingWithComps;
            if (twc != null)
            {
                CustomThingDef colorDef = twc.def as CustomThingDef;
                if (colorDef != null && !colorDef.followStuffColor)
                {
                    twc.SetColor(Color.white);
                    twc.SetColor(Color.black);
                    twc.SetColor(Color.white);
                }
            }
        }
    }

    public class CustomThingDef : ThingDef
    {
        public bool followStuffColor = true;
    }
}

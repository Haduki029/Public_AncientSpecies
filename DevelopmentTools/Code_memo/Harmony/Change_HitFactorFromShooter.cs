using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace SampleCode
{
    // 射撃の命中率にパッチ
    [HarmonyPatch(typeof(ShotReport), "HitFactorFromShooter")]
    [HarmonyPatch(new Type[]
    {
        typeof(Thing),
        typeof(float),
    })]
    internal static class HitFactorFromShooter_Patch
    {

        [HarmonyPostfix]
        static void Postfix(ref float __result, Thing caster, float distance)
        {
            if (caster is Pawn)
            {
                Pawn pawn = (Pawn)caster;
                if (pawn.health.hediffSet.HasHediff(Hediff_LitF.LitF_Resanagia_Curse))
                {
                    __result -= 0.5f;
                }
            }
            return;
        }
    }


}

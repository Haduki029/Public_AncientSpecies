using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace AncientSpecies
{
    public class Patch_SupressIllegalPawnGenerate : Mod
    {
        public Patch_SupressIllegalPawnGenerate(ModContentPack content) : base(content)
        {
            if (ModsConfig.IsActive("raceQuestPawn")) return;
            Log.Message("Patch_SupressIllegalPawnGenerate is started.", false);
            var harmony = new Harmony("rimworld.hadki.ancient_species");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn")]
    [HarmonyPatch(new Type[] { typeof(PawnGenerationRequest) })]
    public class GeneratePawn_Patch
    {
        [HarmonyAfter(new string[] { "rimworld.erdelf.alien_race.main" })]
        public static void Prefix(ref PawnGenerationRequest request)
        {
            // Real Faction Guestが導入されている場合は実行せず終了
            if (ModsConfig.IsActive("raceQuestPawn")) return;
            // gemeの状態がプレイ中でなければ実行せず終了
            if (Current.ProgramState != ProgramState.Playing) return;
            // RacePropsがnullか、AnimalもしくはToolUser以下の値だった場合実行せず終了
            if (request.KindDef.RaceProps != null && (
                request.KindDef.RaceProps.Animal
                || request.KindDef.RaceProps.intelligence <= Intelligence.ToolUser
                )) return;
            // プレイヤー派閥の場合実行せず終了
            if (request.Faction != null && request.Faction.IsPlayer) return;

            Log.Message(
                $"Patch_SupressIllegalPawnGenerate request : {(request.Faction != null ? request.Faction.def.defName : "none")}, " +
                $"{(request.KindDef != null ? request.KindDef.defName : "none")}");

            if (request.Faction != null && !request.Faction.def.modContentPack.PackageId.Contains("ludeon") &&
                request.KindDef.modContentPack.PackageId.Contains("ludeon"))
            {

            }
                return;
        }
    }
}

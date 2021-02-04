using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;
using UnityEngine;
using HarmonyLib;
using RimWorld.Planet;

namespace AS_WHE
{
    public class StockGen_WHE_Slaves : StockGenerator
    {
        private bool respectPopulationIntent = false;
        public override IEnumerable<Thing> GenerateThings(int forTile, Faction faction = null)
        {
            if (this.respectPopulationIntent && Rand.Value > StorytellerUtilityPopulation.PopulationIntent)
            {
                yield break;
            }
            int count = this.countRange.RandomInRange;
            for (int i = 0; i < count; i++)
            {
                Faction wHE_SlaveFaction;
                if (!(from fac in Find.FactionManager.AllFactionsVisible
                      where fac != Faction.OfPlayer && fac.def.humanlikeFaction
                      select fac).TryRandomElement(out wHE_SlaveFaction))
                {
                    yield break;
                }
                PawnGenerationRequest request = AS_WHE_PawnDefault(AS_WHE_PawnKindDefOf.AS_WHE_Slave);
                request.ForcedTraits = TraitsGenerate();
                request.Faction = wHE_SlaveFaction;
                request.Tile = forTile;
                request.ForceAddFreeWarmLayerIfNeeded = !this.trader.orbital;
                request.RedressValidator = ((Pawn x) => x.royalty == null || !x.royalty.AllTitlesForReading.Any<RoyalTitle>());
                request.FixedGender = Gender.Female;

                yield return PawnGenerator.GeneratePawn(request);
            }
            yield break;
        }

        public override bool HandlesThingDef(ThingDef thingDef)
        {
            return thingDef.category == ThingCategory.Pawn && thingDef.race.Humanlike && thingDef.tradeability != Tradeability.None;
        }

        private static PawnGenerationRequest AS_WHE_PawnDefault(PawnKindDef kind)
        {
            return new PawnGenerationRequest
            {
                KindDef = kind,
                Context = PawnGenerationContext.NonPlayer,
                Tile = -1,
                CanGeneratePawnRelations = true,
                ColonistRelationChanceFactor = 0.3f,
                AllowGay = true,
                AllowFood = true,
                AllowAddictions = true,
                RelationWithExtraPawnChanceFactor = 0.3f
            };
        }

        private static IEnumerable<TraitDef> TraitsGenerate()
        {
            return null;
        }
    }
}

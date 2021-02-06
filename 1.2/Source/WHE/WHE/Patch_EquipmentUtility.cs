using HarmonyLib;
using RimWorld;
using System;
using Verse;

namespace AS_WHE
{
	[StaticConstructorOnStartup]
	public static class Patch_EquipmentUtility
	{
		static Patch_EquipmentUtility()
		{
			Harmony harmony = new Harmony("LimitApparel.EquipmentUtility.HarmonyPatch");
			harmony.Patch(AccessTools.Method(typeof(EquipmentUtility), "CanEquip", new Type[]
			{
				typeof(Thing),
				typeof(Pawn),
				typeof(string).MakeByRefType()
			}, null), null, new HarmonyMethod(patchType, "CanEquipPostfix", null), null, null);
		}

		public static void CanEquipPostfix(ref bool __result, Thing thing, Pawn pawn, ref string cantReason)
		{
			Log.Message("Ancient Species: Limit Apparel Check.");
			if (__result)
			{
				Log.Message("Ancient Species: Limit Apparel Check inline process.");
				LimitApparel limitApparel;
				limitApparel = thing.def.GetModExtension<LimitApparel>();
				//thingからLimitApparelを引っ張ってくる必要がある
				//現時点ではthingが不明なためnullを返す
				if (thing.def.IsApparel && limitApparel != null && !limitApparel.CorrectBodyTypeForWearing(pawn.story.bodyType))
				{
					__result = false;
					cantReason = string.Format("WHE_CantWear".Translate(), pawn);
					return;
				}
				//武器も制限は可能。でも体型で制限される武器ってある？？
				/*if (thing.def.IsWeapon && !RaceRestrictionSettings.CanEquip(thing.def, pawn.def))
				{
					__result = false;
					cantReason = string.Format("{0} can't equip this", pawn.def.LabelCap);
					return;
				}*/
			}
		}

		private static readonly Type patchType = typeof(Patch_EquipmentUtility);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace AS_WHE
{
    public class ITab_WHE_Tech : ITab
    {
        private float viewHeight = 1000f;

        private Vector2 scrollPosition = default(Vector2);

        private WorkPlan mouseoverPlan;

        private static readonly Vector2 WinSize = new Vector2(420f, 480f);

        [TweakValue("Interface", 0f, 128f)]
        private static float PasteX = 48f;

        [TweakValue("Interface", 0f, 128f)]
        private static float PasteY = 3f;

        [TweakValue("Interface", 0f, 32f)]
        private static float PasteSize = 24f;

        public ITab_WHE_Tech()
        {
            this.size = ITab_WHE_Tech.WinSize;
            this.labelKey = "WHE_Tech_TabCategory".Translate();
            this.tutorTag = "技術カテゴリー";
        }

        public CompWHERace CompRace
        {
            get
            {
                return SelPawn.GetComp<CompWHERace>();
            }

        }

        protected override void FillTab()
        {
            Rect rect = new Rect(ITab_WHE_Tech.WinSize.x - ITab_WHE_Tech.PasteX, ITab_WHE_Tech.PasteY, ITab_WHE_Tech.PasteSize, ITab_WHE_Tech.PasteSize);
            Rect rect2 = new Rect(0f, 0f, ITab_WHE_Tech.WinSize.x, ITab_WHE_Tech.WinSize.y).ContractedBy(10f);
            Func<List<FloatMenuOption>> recipeOptionsMaker = delegate
            {
                List<FloatMenuOption> list = new List<FloatMenuOption>();
                if (!list.Any<FloatMenuOption>())
                {
                    list.Add(new FloatMenuOption("NoneBrackets".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null));
                }
                return list;
            };
            this.mouseoverPlan = this.CompRace.policy.DoListing(rect2, recipeOptionsMaker, ref this.scrollPosition, ref this.viewHeight);
        }

    }




}

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
    public class AS_WorkPlanStack : IExposable
    {
        [Unsaved]
        public CompWHERace compRace;

        public List<WorkPlan> plans = new List<WorkPlan>();

        public AS_WorkPlanStack(CompWHERace Comp)
        {
            this.compRace = Comp;
        }

        public void ExposeData()
        {
            Scribe_Collections.Look<WorkPlan>(ref this.plans, "plans", LookMode.Deep, new object[0]);
            if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
            {
                if (this.plans.RemoveAll((WorkPlan x) => x == null) != 0)
                {
                    Log.Error("Some manaSqueezePolicy were null after loading");
                }
                for (int i = 0; i < this.plans.Count; i++)
                {
                    this.plans[i].workPlanStack = this;
                }
            }
        }

        public void Reorder(WorkPlan plan, int offset)
        {
            int num = this.plans.IndexOf(plan);
            num += offset;
            if (num >= 0)
            {
                this.plans.Remove(plan);
                this.plans.Insert(num, plan);
            }
        }
        public int IndexOf(WorkPlan plan)
        {
            return this.plans.IndexOf(plan);
        }

        public WorkPlan DoListing(Rect rect, Func<List<FloatMenuOption>> recipeOptionsMaker, ref Vector2 scrollPosition, ref float viewHeight)
        {
            WorkPlan result = null;
            GUI.BeginGroup(rect);
            Rect rect2 = new Rect(0f, 0f, 150f, 29f);
            Rect rect2b = new Rect(180f, 5f, 150f, 24f);
            Text.Font = GameFont.Medium;
            Widgets.Label(rect2, "魔力収集方針");
            Text.Font = GameFont.Small;
            Widgets.Label(rect2b, "獲得した魔力：" + compRace.TotalManaGained.ToString("F2"));
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.color = Color.red;
            Rect outRect = new Rect(0f, 35f, rect.width, rect.height - 35f);
            Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, viewHeight);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect, true);
            float num = 0f;
            for (int i = 0; i < this.plans.Count; i++)
            {
                WorkPlan plan = this.plans[i];
                Rect rect3 = plan.DoInterface(0f, num, viewRect.width, i);
                if (Mouse.IsOver(rect3))
                {
                    result = plan;
                }
                num += rect3.height + 6f;
            }
            if (Event.current.type == EventType.Layout)
            {
                viewHeight = num + 60f;
            }
            Widgets.EndScrollView();
            GUI.EndGroup();
            return result;
        }
    }

    public class WorkPlan : IExposable, ILoadReferenceable
    {
        public int loadID = -1;
        [Unsaved]
        public AS_WorkPlanStack workPlanStack;
        public bool Suspended;
        public RelationType targetType;
        public bool AllowMeditation;          //瞑想の許可
        public float SaveUpValue = 0.8f;     //0.4～2.4

        public WorkPlan() //初期化
        {
            
        }

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.AllowMeditation, "AllowMeditation", false, false);
            Scribe_Values.Look<float>(ref this.SaveUpValue, "SaveUpValue", 0.8f, false);
        }

        public Rect DoInterface(float x, float y, float width, int index)
        {
            Rect rect = new Rect(x, y, width, 53f);
            rect.height += 24f;
            Color white = Color.white;
            if (Suspended)
            {
                white = new Color(1f, 0.7f, 0.7f, 0.7f);
            }
            GUI.color = white;
            Text.Font = GameFont.Small;
            if (index % 2 == 0)
            {
                Widgets.DrawAltRect(rect);
            }
            GUI.BeginGroup(rect);
            //優先順アイコン
            Rect rect2 = new Rect(0f, 0f, 24f, 24f);
            if (this.workPlanStack.IndexOf(this) > 0)
            {
                if (Widgets.ButtonImage(rect2, TexButton.ReorderUp, white))
                {
                    this.workPlanStack.Reorder(this, -1);
                    SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
                }
                TooltipHandler.TipRegion(rect2, "ReorderBillUpTip".Translate());
            }
            if (this.workPlanStack.IndexOf(this) < this.workPlanStack.plans.Count - 1)
            {
                Rect rect3 = new Rect(0f, 48f, 24f, 24f);
                if (Widgets.ButtonImage(rect3, TexButton.ReorderDown, white))
                {
                    this.workPlanStack.Reorder(this, 1);
                    SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
                }
                TooltipHandler.TipRegion(rect3, "ReorderBillDownTip".Translate());
            }
            Rect rect4 = new Rect(28f, 0f, rect.width - 48f - 20f, rect.height + 5f);
            Widgets.Label(rect4, this.LabelCap);
            Rect rect5 = new Rect(rect.width - 24f, 0f, 24f, 24f);
            Rect rect7 = new Rect(rect5);
            rect7.x -= rect7.width + 4f;
            if (Widgets.ButtonImage(rect7, TexButton.Suspend, white))
            {
                this.Suspended = !this.Suspended;
                SoundDefOf.Click.PlayOneShotOnCamera(null);
            }
            TooltipHandler.TipRegion(rect7, "SuspendBillTip".Translate());
           
            GUI.EndGroup();
            if (this.Suspended)
            {
                Text.Font = GameFont.Medium;
                Text.Anchor = TextAnchor.MiddleCenter;
                Rect rect9 = new Rect(rect.x + rect.width / 2f - 70f, rect.y + rect.height / 2f - 20f, 140f, 40f);
                GUI.DrawTexture(rect9, TexUI.GrayTextBG);
                Widgets.Label(rect9, "SuspendedCaps".Translate());
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            return rect;
        }

        public string GetUniqueLoadID()
        {
            return string.Concat(new object[]
            {
                    "Plan_",
                    "_",
                    loadID
            });
        }

        public string LabelCap
        {
            get
            {
                switch (targetType)
                {
                    case RelationType.Meal:
                        return "NItxt_Meal".Translate();
                    case RelationType.Colonist:
                        return "NItxt_Colonist".Translate();
                    case RelationType.Prisoner:
                        return "NItxt_Prisoner".Translate();
                    case RelationType.Hostile:
                        return "NItxt_Enemy".Translate();
                    default:
                        return "NItxt_Other".Translate();
                }


            }
        }

    }

    public enum RelationType
    {
        Colonist = 0,
        Prisoner = 1,
        Other = 2,
        Ally = 3,
        Neutral = 4,
        Hostile = 5,
        Meal = 6
    }

    [StaticConstructorOnStartup]
    internal class TexButton
    {
        public static readonly Texture2D ReorderUp = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderUp", true);

        public static readonly Texture2D ReorderDown = ContentFinder<Texture2D>.Get("UI/Buttons/ReorderDown", true);

        public static readonly Texture2D Suspend = ContentFinder<Texture2D>.Get("UI/Buttons/Suspend", true);

    }
}

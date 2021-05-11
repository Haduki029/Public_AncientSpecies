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
    //サキュバス種族Comp
    public class CompProperties_WHERace : CompProperties
    {
        public CompProperties_WHERace()
        {
            this.compClass = typeof(CompWHERace);
        }
    }
    public class CompWHERace : ThingComp
    {
        public float ManaFallPerTick = 0.00001f; //一日に0.6消費
        public float ManaFallFactor = 1f;
        public float JoyThreshold = 0.5f;
        public float JoyTolerancesFactor = 1f;
        public AS_WorkPlanStack policy;
        public float TotalManaGained; //回収したマナ総量。最大値9999.00

        public const float JoyModeSqueezeAmount = 0.2f;

        public CompProperties_WHERace Props
        {
            get
            {
                //Log.Message("Ancient Species: (CompProperties_WHERace)this.props = " + (CompProperties_WHERace)this.props);
                return (CompProperties_WHERace)this.props;
            }
        }

        public Pawn Pawn
        {
            get
            {
                return this.parent as Pawn;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Log.Message("Ancient Species: Expose");
            Scribe_Values.Look<float>(ref TotalManaGained, "TotalManaGained", 0f, false);
            Scribe_Deep.Look(ref policy, "ManaSqueezePolicy", new object[] { this });
        }
        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            Log.Message("Init");
            policy = new AS_WorkPlanStack(this);
            //Log.Message("Ancient Species: CompProperties_WHERace Class policy proparty = " + policy);
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            int manaAttribute = Pawn.story.traits.DegreeOfTrait(WHETraitDefOf.AS_WeebElf_OdTrait);
            switch (manaAttribute)
            {
                case 1:
                    ManaFallFactor = 0.5f;
                    JoyThreshold = 0.3f;
                    break;
                case 2:
                    ManaFallFactor = 1.0f;
                    break;
                case 3:
                    ManaFallFactor = 1.0f;
                    JoyThreshold = 0.7f;
                    JoyTolerancesFactor = 1.25f;
                    break;
                case 4:
                    ManaFallFactor = 2.0f;
                    JoyThreshold = 0.7f;
                    break;
                case 5:
                    ManaFallFactor = 2.0f;
                    JoyThreshold = 0.7f;
                    JoyTolerancesFactor = 0.5f;
                    break;
                case 6:
                    ManaFallFactor = 5.0f;
                    break;
            }
            if (Pawn.story != null && Pawn.story.traits.HasTrait(TraitDefOf.Ascetic)) JoyThreshold = 0f;
            InitManaSqueezePolicy();

        }

        public void InitManaSqueezePolicy(bool forcedInit = false)
        {
            //Log.Message("start");

            if (forcedInit || !this.policy.plans.Any())
            {
                //policy = new WorkPlanStack(this);
                policy.plans.Clear();
                int manaAttribute = Pawn.story.traits.DegreeOfTrait(WHETraitDefOf.AS_WeebElf_OdTrait);
                WorkPlan forMeal = new WorkPlan()
                {
                    workPlanStack = this.policy,
                    targetType = RelationType.Meal
                };
                WorkPlan forColonist = new WorkPlan()
                {
                    workPlanStack = this.policy,
                    targetType = RelationType.Colonist,
                    AllowMeditation = true,
                };
                WorkPlan forPrisoner = new WorkPlan()
                {
                    workPlanStack = this.policy,
                    targetType = RelationType.Prisoner,
                    AllowMeditation = true,
                };
                WorkPlan forEnemy = new WorkPlan()
                {
                    workPlanStack = this.policy,
                    targetType = RelationType.Hostile,
                    AllowMeditation = true,
                    SaveUpValue = 2.1f
                };
                WorkPlan forOther = new WorkPlan()
                {
                    workPlanStack = this.policy,
                    targetType = RelationType.Other,
                    AllowMeditation = true,
                };

                switch (manaAttribute)
                {
                    case 1: //静寂型
                        forColonist.AllowMeditation = false;
                        forColonist.SaveUpValue = 0.7f;
                        forPrisoner.AllowMeditation = false;
                        forPrisoner.SaveUpValue = 0.7f;
                        forEnemy.AllowMeditation = false;
                        forEnemy.SaveUpValue = 1f;
                        forOther.AllowMeditation = false;
                        forOther.SaveUpValue = 0.7f;
                        policy.plans = new List<WorkPlan>()
                        {
                            forMeal,
                            forColonist,
                            forPrisoner,
                            forEnemy,
                            forOther
                        };
                        break;
                    case 2:
                    case 3:
                        policy.plans = new List<WorkPlan>()
                        {
                            forMeal,
                            forEnemy,
                            forPrisoner,
                            forColonist,
                            forOther
                        };
                        break;
                    case 4:
                        forPrisoner.SaveUpValue = 1.2f;
                        policy.plans = new List<WorkPlan>()
                        {
                            forMeal,
                            forEnemy,
                            forPrisoner,
                            forColonist,
                            forOther
                        };
                        break;
                    case 5:
                        forOther.SaveUpValue = 1.5f;
                        policy.plans = new List<WorkPlan>()
                        {
                            forMeal,
                            forEnemy,
                            forPrisoner,
                            forColonist,
                            forOther
                        };
                        break;
                    case 6:
                        forOther.SaveUpValue = 1.5f;
                        policy.plans = new List<WorkPlan>()
                        {
                            forMeal,
                            forEnemy,
                            forPrisoner,
                            forColonist,
                            forOther
                        };
                        break;
                    default:
                        policy.plans = new List<WorkPlan>()
                        {
                            forMeal,
                            forEnemy,
                            forPrisoner,
                            forColonist,
                            forOther
                        };
                        break;
                }

            }
            //this.policy.compRace = this;

        }

    }
}

export namespace RaceDef {
    export interface Root {
        Defs: Defs;
    }
    
    export interface Defs {
        ThingDef:                       ThingDef;
        "AlienRace.ThingDef_AlienRace": AlienRaceThingDefAlienRace;
    }
    
    export interface AlienRaceThingDefAlienRace {
        defName:     string;
        label:       string;
        description: string;
        alienRace:   AlienRace;
        statBases:   AlienRaceThingDefAlienRaceStatBases;
        tools:       Tools;
        race:        Race;
        recipes:     Recipes;
        _ParentName: string;
    }
    
    export interface AlienRace {
        generalSettings: GeneralSettings;
        graphicPaths:    GraphicPaths;
        styleSettings:   StyleSettings;
        raceRestriction: RaceRestriction;
    }
    
    export interface GeneralSettings {
        maleGenderProbability:   string;
        immuneToAge:             string;
        canLayDown:              string;
        forcedRaceTraitEntries:  ForcedRaceTraitEntries;
        disallowedTraits:        DisallowedTraits;
        maxDamageForSocialfight: string;
        humanRecipeImport:       string;
        immuneToXenophobia:      string;
        factionRelations:        FactionRelations;
        alienPartGenerator:      AlienPartGenerator;
    }
    
    export interface AlienPartGenerator {
        headOffset:                 string;
        customDrawSize:             string;
        customHeadDrawSize:         string;
        customPortraitDrawSize:     string;
        customPortraitHeadDrawSize: string;
        aliencrowntypes:            HediffGiverSets;
        alienbodytypes:             Recipes;
        useGenderedHeads:           string;
        useGenderedBodies:          string;
    }
    
    export interface Recipes {
        li: string;
    }
    
    export interface HediffGiverSets {
        li: string[];
    }
    
    export interface DisallowedTraits {
        li: DisallowedTraitsLi[];
    }
    
    export interface DisallowedTraitsLi {
        defName: string;
    }
    
    export interface FactionRelations {
        li: FactionRelationsLi;
    }
    
    export interface FactionRelationsLi {
        factions: HediffGiverSets;
        goodwill: Goodwill;
    }
    
    export interface Goodwill {
        min: string;
        max: string;
    }
    
    export interface ForcedRaceTraitEntries {
        li: ForcedRaceTraitEntriesLi[];
    }
    
    export interface ForcedRaceTraitEntriesLi {
        defName: string;
        degree:  string;
        chance:  string;
    }
    
    export interface GraphicPaths {
        li: GraphicPathsLi;
    }
    
    export interface GraphicPathsLi {
        head: string;
        body: string;
    }
    
    export interface RaceRestriction {
        apparelList:                  HediffGiverSets;
        whiteApparelList:             Recipes;
        onlyUseRaceRestrictedApparel: string;
        onlyUseRaceRestrictedWeapons: string;
    }
    
    export interface StyleSettings {
        li: StyleSettingsLi[];
    }
    
    export interface StyleSettingsLi {
        key:   string;
        value: Value;
    }
    
    export interface Value {
        hasStyle:           string;
        styleTagsOverride?: Recipes;
        styleTags?:         Recipes;
    }
    
    export interface Race {
        thinkTreeMain:             string;
        thinkTreeConstant:         string;
        intelligence:              string;
        makesFootprints:           string;
        lifeExpectancy:            string;
        leatherDef:                string;
        nameCategory:              string;
        body:                      string;
        baseBodySize:              string;
        baseHealthScale:           string;
        foodType:                  string;
        gestationPeriodDays:       string;
        meatMarketValue:           string;
        manhunterOnDamageChance:   string;
        manhunterOnTameFailChance: string;
        litterSizeCurve:           Curve;
        lifeStageAges:             LifeStageAges;
        soundMeleeHitPawn:         string;
        soundMeleeHitBuilding:     string;
        soundMeleeMiss:            string;
        specialShadowData:         SpecialShadowData;
        ageGenerationCurve:        Curve;
        hediffGiverSets:           HediffGiverSets;
    }
    
    export interface Curve {
        points: HediffGiverSets;
    }
    
    export interface LifeStageAges {
        li: LifeStageAgesLi[];
    }
    
    export interface LifeStageAgesLi {
        def:    string;
        minAge: string;
    }
    
    export interface SpecialShadowData {
        volume: string;
        offset: string;
    }
    
    export interface AlienRaceThingDefAlienRaceStatBases {
        MarketValue:                 string;
        MoveSpeed:                   string;
        MentalBreakThreshold:        string;
        ComfyTemperatureMax:         string;
        ComfyTemperatureMin:         string;
        ImmunityGainSpeed:           string;
        ToxicSensitivity:            string;
        CarryingCapacity:            string;
        MeatAmount:                  string;
        LeatherAmount:               string;
        PainShockThreshold:          string;
        ShootingAccuracyPawn:        string;
        AimingDelayFactor:           string;
        ArmorRating_Blunt:           string;
        MeleeDodgeChance:            string;
        WorkSpeedGlobal:             string;
        NegotiationAbility:          string;
        SellPriceFactor:             string;
        SocialImpact:                string;
        TameAnimalChance:            string;
        TrainAnimalChance:           string;
        AnimalGatherSpeed:           string;
        AnimalGatherYield:           string;
        MiningSpeed:                 string;
        DeepDrillingSpeed:           string;
        HuntingStealth:              string;
        ButcheryMechanoidEfficiency: string;
        ButcheryMechanoidSpeed:      string;
        ButcheryFleshEfficiency:     string;
        ButcheryFleshSpeed:          string;
        FoodPoisonChance:            string;
        CookSpeed:                   string;
        PlantWorkSpeed:              string;
        PlantHarvestYield:           string;
        ResearchSpeed:               string;
    }
    
    export interface Tools {
        li: ToolsLi[];
    }
    
    export interface ToolsLi {
        label:                                   string;
        capacities:                              Recipes;
        power:                                   string;
        cooldownTime:                            string;
        linkedBodyPartsGroup:                    string;
        surpriseAttack?:                         SurpriseAttack;
        chanceFactor?:                           string;
        ensureLinkedBodyPartsGroupAlwaysUsable?: string;
    }
    
    export interface SurpriseAttack {
        extraMeleeDamages: ExtraMeleeDamages;
    }
    
    export interface ExtraMeleeDamages {
        li: ExtraMeleeDamagesLi;
    }
    
    export interface ExtraMeleeDamagesLi {
        def:    string;
        amount: string;
    }
    
    export interface ThingDef {
        statBases:   ThingDefStatBases;
        _Name:       string;
        _ParentName: string;
        _Abstract:   string;
    }
    
    export interface ThingDefStatBases {
        Mass:         string;
        Flammability: string;
    }
    
}
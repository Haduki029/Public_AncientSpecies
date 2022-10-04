export namespace ApparelItem {
    export interface Root {
        Defs: Defs;
    }
    
    export interface Defs {
        ThingDef: ThingDef[];
    }
    
    export interface ThingDef {
        defName:             string;
        description:         string;
        label:               string;
        techLevel:           string;
        graphicData:         GraphicData;
        followStuffColor:    string;
        costStuffCount:      string;
        stuffCategories:     StuffCategories;
        costList:            CostList;
        statBases:           StatBases;
        equippedStatOffsets: EquippedStatOffsets;
        recipeMaker:         RecipeMaker;
        apparel:             Apparel;
    }
    
    export interface Apparel {
        bodyPartGroups:    StuffCategories;
        wornGraphicPath:   string;
        layers:            DefaultOutfitTags;
        tags:              StuffCategories;
        defaultOutfitTags: DefaultOutfitTags;
    }
    
    export interface StuffCategories {
        li: string[];
    }
    
    export interface DefaultOutfitTags {
        li: string[];
    }
    
    export interface CostList {
        Hyperweave: string;
        Gold:       string;
        Plasteel:   string;
    }
    
    export interface EquippedStatOffsets {
        MoveSpeed:              string;
        AccuracyTouch:          string;
        AimingDelayFactor:      string;
        ArmorRating_Blunt:      string;
        ArmorRating_Sharp:      string;
        MeleeHitChance:         string;
        MeleeDodgeChance:       string;
        PawnTrapSpringChance:   string;
        ForagedNutritionPerDay: string;
        MentalBreakThreshold:   string;
        SocialImpact:           string;
        CarryingCapacity:       string;
    }
    
    export interface GraphicData {
        texPath:      string;
        graphicClass: string;
    }
    
    export interface RecipeMaker {
        recipeUsers:          RecipeUsers;
        researchPrerequisite: string;
    }
    
    export interface RecipeUsers {
        li:       string[];
        _Inherit: string;
    }
    
    export interface StatBases {
        MaxHitPoints:                         string;
        WorkToMake:                           string;
        StuffEffectMultiplierArmor:           string;
        StuffEffectMultiplierInsulation_Cold: string;
        StuffEffectMultiplierInsulation_Heat: string;
        ArmorRating_Sharp:                    string;
        ArmorRating_Blunt:                    string;
        ArmorRating_Heat:                     string;
        Insulation_Cold:                      string;
        Insulation_Heat:                      string;
        Mass:                                 string;
        Flammability:                         string;
        EquipDelay:                           string;
    }
    
}
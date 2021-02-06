using System;
using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AS_WHE
{
    public class CompProperties_AT_Item : CompProperties, IExposable
    {
        // Enhancement Stats (%)
        public float maxMP = 0;
        public float rangeWeaponCoolDownEnhancement = 0;
        public float meleeWeaponCoolDownEnhancement = 0;
        public float rangeWeaponDamageEnhancement = 0;
        public float meleeWeaponDamageEnhancement = 0;
        public float hPRegenRate = 0;
        public float healthRegenRate = 0;


        public void ExposeData()
        {
            Scribe_Values.Look<float>(ref this.maxMP, "maxMP", 0, false);
            Scribe_Values.Look<float>(ref this.rangeWeaponCoolDownEnhancement, "rangeWeaponCoolDownEnhancementP", 0, false);
            Scribe_Values.Look<float>(ref this.meleeWeaponCoolDownEnhancement, "meleeWeaponCoolDownEnhancementP", 0, false);
            Scribe_Values.Look<float>(ref this.rangeWeaponDamageEnhancement, "rangeWeaponDamageEnhancementP", 0, false);
            Scribe_Values.Look<float>(ref this.meleeWeaponDamageEnhancement, "meleeWeaponDamageEnhancementP", 0, false);
            Scribe_Values.Look<float>(ref this.hPRegenRate, "hPRegenRateP", 0, false);
            Scribe_Values.Look<float>(ref this.healthRegenRate, "healthRegenRateP", 0, false);
        }
        public CompProperties_AT_Item()
        {
            this.compClass = typeof(CompEnchantedItem);
            //this.AbilityUserClass = typeof(GenericCompAbilityUser);
        }

    }
}

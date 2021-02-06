using RimWorld;
using System.Collections.Generic;
using Verse;

namespace AS_WHE
{
    public class LimitApparel : DefModExtension
    {
        public bool CorrectBodyTypeForWearing(BodyTypeDef bodyTypeDef)
        {
            Log.Message("Ancient Species: Limit Apparel.");
            return matchBodyType == null || matchBodyType.Contains(bodyTypeDef);
        }

        public List<BodyTypeDef> matchBodyType;
    }
}

[StaticConstructorOnStartup]
    [HarmonyPatch(typeof(Pawn_HealthTracker))]
    class Pawn_HealthTracker_Patch
    {
        /// <summary>
        /// 値の受け渡し用構造体。
        /// </summary>
        struct StateParam
        {
            public bool flag1;
            public bool flag2;
            public StateParam(bool flag1, bool flag2)
            {
                this.flag1 = flag1;
                this.flag2 = flag2;
            }
        }
        /// <summary>
        /// Prefix1
        /// 対応するPostfix1と同じクラスに定義されていないと__stateの受け渡しができません。
        /// </summary>
        /// <param name="def">HediffDef</param>
        /// <param name="__state">Prefix-Postfix連携用引数。受け渡し用なのでrefじゃないとだめです(Prefixの方はoutでもいいです)。値を複数渡したい場合は今回は構造体ですが別にクラスでもいいです。なんならコレクションでもタプルでもいいですし、単一の値ならその型のみでいいです。要はなんでもいいです。</param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Pawn_HealthTracker.AddHediff))]
        [HarmonyPatch(new Type[] { typeof(HediffDef), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult) })]
        static bool AddHediff_Prefix1(HediffDef def, ref StateParam __state)
        {
            bool flag1 = Rand.Int % 2 == 0;
            bool flag2 = !flag1;
            __state = new StateParam(flag1, flag2);
            Log.Message($"AddHediff_Prefix1: hediffDef={def.LabelCap} flag1={__state.flag1}, flag2={__state.flag2}");
            return true;
        }
        /// <summary>
        /// Prefix2
        /// 対応するPostfix2と同じクラスに定義されていないと__stateの受け渡しができません。
        /// </summary>
        /// <param name="hediff">Hediff</param>
        /// <param name="__state">Prefix-Postfix連携用引数。</param>
        /// <returns></returns>
        [HarmonyPrefix]
        [HarmonyPatch(nameof(Pawn_HealthTracker.AddHediff))]
        [HarmonyPatch(new Type[] { typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult) })]
        static bool AddHediff_Prefix2(Hediff hediff, out StateParam __state) //Prefixは初期化必須なのでref/outどちらで宣言してもいいです。
        {
            bool flag1 = Rand.Int % 2 == 0;
            bool flag2 = !flag1;
            __state = new StateParam(flag1, flag2);
            Log.Message($"AddHediff_Prefix2: hediff={hediff}, flag1={__state.flag1}, flag2={__state.flag2}");
            return true;
        }

        /// <summary>
        /// Postfix1
        /// 対応するPrefix1と同じクラスに定義されていないと__stateの受け取りができません。
        /// </summary>
        /// <param name="def">HediffDef</param>
        /// <param name="__state">Prefix1で初期化されてrefで参照渡しされてきた構造体</param>
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Pawn_HealthTracker.AddHediff))]
        [HarmonyPatch(new Type[] { typeof(HediffDef), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult) })]
        static void AddHediff_Postfix1(HediffDef def, ref StateParam __state)
        {
            Log.Message($"AddHediff_Postfix1: hediffDef={def.LabelCap} flag1={__state.flag1}, flag2={__state.flag2}");
        }
        /// <summary>
        /// Postfix2
        /// 対応するPrefix2と同じクラスに定義されていないと__stateの受け取りができません。
        /// </summary>
        /// <param name="hediff">Hediff</param>
        /// <param name="__state">Prefix2で初期化されてrefで参照渡しされてきた構造体</param>
        [HarmonyPostfix]
        [HarmonyPatch(nameof(Pawn_HealthTracker.AddHediff))]
        [HarmonyPatch(new Type[] { typeof(Hediff), typeof(BodyPartRecord), typeof(DamageInfo?), typeof(DamageWorker.DamageResult) })]
        static void AddHediff_Postfix2(Hediff hediff, ref StateParam __state)
        {
            Log.Message($"AddHediff_Postfix2: hediff={hediff}, flag1={__state.flag1}, flag2={__state.flag2}");
        }
    }
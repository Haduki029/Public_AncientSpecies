using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RimWorld;
using Verse;

namespace AS_WHE
{
    /// <summary>
    /// CompPropertiesを継承したクラスです。
    /// XMLから値を取得してCompクラスに引き渡します。
    /// </summary>
    public class CompProperties_GenderPurge : CompProperties
    {
        /// <summary>
        /// 装備を許可する性別を指定します
        /// </summary>
        public Gender gender = Gender.Female;

        public CompProperties_GenderPurge()
        {
            this.compClass = typeof(CompGenderPurge);
        }
    }
    /// <summary>
    /// ThingCompを継承したCompの本体です。
    /// CompPropertiesから引き受けた値を元に各種処理を行います。 
    /// </summary>
    public class CompGenderPurge : ThingComp
    {
        /// <summary>
        /// CompPropertiesから値を引き受けます。
        /// 以降、Propsから値を抽出できます。
        /// </summary>
        public CompProperties_GenderPurge Props
        {
            get
            {
                return (CompProperties_GenderPurge)this.props;
            }
        }


        public override void CompTick()
        {
            this.Tick(1);
        }
        /// <summary>
        /// Tick処理
        /// ゲーム内のTick毎にここの処理を実行します。
        /// </summary>
        /// <param name="interval"></param>
        public void Tick(int interval)
        {
            // これは衣服か？
            Apparel apparel = this.parent as Apparel;
            if (apparel == null)
            {
                // 衣服ではない場合は処理を実行しない
                return;
            }
            // 装備者はPawnか？
            Pawn WearPawn = apparel.Wearer as Pawn;
            if (WearPawn == null)
            {
                // 装備者不在時は処理を実行しない
                return;
            }
            if (WearPawn.Dead == true)
            {
                // 対象が死んでいる場合は処理を実行しない
                return;
            }
            // XMLで指定した性別とは異なる場合、装備を解除する
            if (WearPawn.gender != Props.gender)
            {
                // 装備解除処理
                if (WearPawn.apparel.TryDrop(apparel, out Apparel resultingAp, WearPawn.Position.RandomAdjacentCell8Way(), false) == true)
                {
                    // 装備をDropした。
                    Log.Message(WearPawn.Name.ToStringShort + "は性別が不一致のため" + resultingAp.ToString() + "を装備できなかった。");
                }
            }
        }
    }
}

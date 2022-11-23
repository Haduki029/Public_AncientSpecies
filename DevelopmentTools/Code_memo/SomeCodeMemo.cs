using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace SampleCode
{
    public class Main
    {
        static Main()
        {
            // ポートレートに写っている状態を取得するなら
            RenderTexture Render =
                PortraitsCache.Get(pawn, new Vector2(1.0f, 1.0f), Rot4.South, new Vector2(0, 0), 1.0f, false, false, true, true);


            /** AssetBundle
              *  1.適当なUnityのプロジェクトを作成
              *  2.そこに読み込みたい画像データとAssetBundle生成スクリプトを入れる
              *  3.AssetBundle生成
              *  4.RimのModに移してAssetBundle.LoadFromFileとLoadAllAssetsで読み込む
              */
            //   https://qiita.com/k7a/items/d27640ac0276214fc850


            // Descriptionの尾末に出展元Mod名を付ける
            // by Animal Dictionary
            foreach(ModContentPack mod in from pack in LoadedModManager.RunningMods
                                          where (pack.IsCoreMod == false)
                                          select pack)
            {
                foreach(Def def in mod.AllDefs)
                {
                    PawnKindDef pkdef = def as PawnKindDef;
                    if((pkdef != null) && (pkdef.defName == selected.defName))
                    {
                        descriptionText += "\n (" + "AD_SourceMod".Translate() + " : " + mod.Name + ")";
                    }
                }
            }

            // できるだけカメラ確認を遅らせてできるだけ軽くする
            if (ticks <= 0 && !YR_RJW_Setting.YR_StopAnimation)
            {
                if (Find.CameraDriver.ZoomRootSize > YR_RJW_Setting.YR_zoom * 6 || !Find.CameraDriver.CurrentViewRect.Contains(parent.Position))
                {
                    return;
                }
            }

            // this.parentが対象のthing

            if (this.parent.IsHashIntervalTick(100))
            {
                Patch_Brank();
            }
            // 例だと100tickに1回Patch_Brankを呼ぶって感じですね


            /**
            * 下コードでそのMODロードされてるか判定できますよぉ。
            * [StaticConstructorOnStartup]属性のついたクラスの静的コンストラクタで行う分にはロード準考慮不要です。
            */
            ModsConfig.IsActive("MODのPackageID");}

            /// <summary>
            /// intervalに入れたTicks毎にtrueになる。必要に応じてdelayを入れる事でtrueになるタイミングに遅延を入れられる
            /// 基本的には一定周期で働かせたいけど動的にタイミングを変えたいな～というそんな貴方にピッタリです！
            /// ディレイには普段は0とし、必要に応じて自然数を代入してください
            /// </summary>
            /// <param name="thing">ハッシュ元</param>
            /// <param name="interval">trueインターバル</param>
            /// <param name="delay">ディレイ</param>
            internal static class TickHelper
            {
                internal static bool AdvancedIsHashIntervalTick(Thing thing, int interval, int delay = 0)
                {
                    int num = delay/interval;
                    return thing.HashOffsetTicks() % (interval * (1+num)) == (delay % interval);
                }
            }
            /**
            * 病気のdefの値にアクセス、値変更
            */
            public void accessAndChangeDefParam(){
                float diseaseminSeveritybackup = disease.def.minSeverity;//この病気の最低進行度値をバックアップしておく。
                float lethalSeveritybackup = disease.def.lethalSeverity;//この病気の致死進行度値をバックアップしておく。
                disease.def.minSeverity = 0f;//この病気の最低進行度を一時的に0に改竄する。
                disease.def.lethalSeverity = 100f;//この病気の致死進行度を一時的に100に改竄する。

                disease.Severity = 1.0f; // "ほにゃほにゃ";

                disease.def.minSeverity = diseaseminSeveritybackup;//病気の最低進行度を元の正常な値に戻す。
                disease.def.lethalSeverity = lethalSeveritybackup; //病気の致死進行度を元の正常な値に戻す。
            }
    }
}


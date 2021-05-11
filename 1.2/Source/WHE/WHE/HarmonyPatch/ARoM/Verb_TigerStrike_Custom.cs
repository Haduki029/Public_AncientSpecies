using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using TorannMagic;
using Verse;

namespace ARoM_CustomPatch
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("rimworld.haduki.ARoM.skill.patch");
            /*
             * 同じインスタンス内（Harmony　Attributeの中身全部）
             */
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    /*
     * 老後のためのHarmonyTranspile
     * ※この解説は、A Rimworld of MagicのコードをDnSpyもしくは、ILSpyを使用して、ILコードが見られる状態であることが前提となってます。
     */
    [HarmonyPatch(typeof(Verb_TigerStrike))]
    [HarmonyPatch("TryCastShot")]
    public class Verb_TigerStrike_Patch
    {
        // こいつはHarmonyTranspilerのアノテーション。メソッド名をTranspilerから名称変えるなら必要だ。別に変えないときに付けておいても問題は出ないぞ。
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> TigerStrikeTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            // 実行コードをlistに突っ込む。こいつはおまじない扱いで良いぞ。
            List<CodeInstruction> list = instructions.ToList<CodeInstruction>();

            /*
             * まず目的だ。これが一番重要だ。
             * お前は今回、A Rimworld of Magicという神Modにある、武術家をぬるゲー環境で使いたくてどうしようもないとしよう。
             * 具体的には、武術家が何らかの武器を装備している場合、TigerStrikeスキルが使えない制限を外してやりたいとなるわけだ。なれ。
             * 
             * 今回の修正は、下記抜粋箇所のflag2の値を常にtrueにしてやりたい。そうすれば武器を装備していてもスキルが使えるようになるわけだ。
             * 
             * [Verb_TigerStrike Classの今回使用箇所の抜粋　以下C#抜粋と表記]
             * protected override bool TryCastShot()
        	 * {
			 *       bool flag = true;
			 *       bool flag2 = this.CasterPawn.equipment.Primary == null;　// <=こいつが諸悪の根源!!!絶許。Trueにしてやる！（してやりたい）
			 *       bool result;
			 *       if (flag2)
			 * {
			 * ...以下省略
             */

            /* 
             * [上記C#抜粋箇所の、ILコード版の抜粋　以下IL抜粋と表記] 
             * 1 : IL_0000: nop
             * 2 : IL_0001: ldc.i4.1
             * 3 : IL_0002: stloc.0
             * 4 : IL_0003: ldarg.0
             * 5 : IL_0004: callvirt  instance class ['Assembly-CSharp']Verse.Pawn ['Assembly-CSharp']Verse.Verb::get_CasterPawn()
             * 6 : IL_0009: ldfld     class ['Assembly-CSharp']Verse.Pawn_EquipmentTracker ['Assembly-CSharp']Verse.Pawn::equipment
             * 7 : IL_000E: callvirt  instance class ['Assembly-CSharp']Verse.ThingWithComps ['Assembly-CSharp']Verse.Pawn_EquipmentTracker::get_Primary()
             * 8 : IL_0013: ldnull
             * 9 : IL_0014: ceq
             * 10: IL_0016: stloc.1
             * 11: IL_0017: ldloc.1
             * 12: IL_0018: brfalse   IL_03E4
             * ...以下省略
             */

            /*
             * [ILコードで表示されるローカル変数リスト　以下IL変数リストと表記]
             * .locals init (
			 * [0] bool,
			 * [1] bool,
			 * [2] class ['Assembly-CSharp']Verse.Thing,
			 * [3] bool,
			 * [4] int32,
			 * [5] class ['Assembly-CSharp']Verse.BodyPartRecord,
			 * [6] class ['Assembly-CSharp']Verse.DamageDef,
			 * ...そもそも今回[1]以外つかわないので以下省略
             */

            // まずはPawnクラスのフィールドequipmentを設定しておけ。理由はあとで解る。
            /*
             * Verb_TigerStrikeのTryCastShotメソッドでは、CasterPawn.equipmentは一か所しか使用されていない。
             * だからお前は、それが使用されている箇所を特定し、その処理の後に無理矢理！値をTrueにしてやればいけるんじゃね？と考えたわけだ。
             * だから特定するために、まずはPawnクラスのフィールドequipmentをf_equipment_Primaryに設定して、場所を特定するための準備をしておくんだ。
             */
            FieldInfo f_equipment_Primary = AccessTools.Field(typeof(Pawn), "equipment");

            // こいつは特定した場所情報を保存しておく変数だ。取り合えず-1を入れとけ。エラー処理とかしやすいから！
           int pos = -1;

            /* 
             * ここでIL抜粋を見ろ。Pawn.equipmentは上から6つ目に記載があるのがわかるだろう。
             * だからまず、listをfor文でぐるぐる回し、狙った命令文がある場所をif文を使って特定するんだ。
             */
            for (int i = 0; i < list.Count - 3; i++)
           {
                /* 
                 * if文の条件は、ldfld且つ、FielldInfoのoperandであることと、f_equipment_Primaryが一致する場所とすればいい。
                 * これでIL抜粋の6の場所が特定できるわけだ。
                 * ※この条件は冗長になっているので、もっと簡潔に記述可能です。
                 */
                if (list[i].opcode == OpCodes.Ldfld && (FieldInfo)list[i].operand == f_equipment_Primary)
                {
                    /*
                     * 今お前は、特定したIL抜粋の6の場所にいる。この後に、Trueを無理矢理！突っ込みたいわけだから、この場所がlistの何番目なのかをposに設定して記憶するんだ。
                     * おっと、その前にTrueを無理矢理！突っ込む場所をもっと具体的にしておこう。
                     * いいか？今回の一連の、武器を持っているか、持っていないかの判断処理は、IL抜粋の5～10の処理が該当する。
                     * つまり、IL抜粋の6の直後にTrueを無理矢理！突っ込むのではなく、判断処理が終わった10の後に突っ込むべきだ。わかるな？
                     * だから命令を割り込ませたいコマンドの位置として、11になるように+5をしてやり、posに記憶させてやれ。
                     * そしたら場所の特定は終わりだから、breakで処理を抜けろ。
                     */
                    pos = i + 5;
                    break;
                }
            }
            /*
             * 補足
             * この後にやりたいこと。
             * [IL抜粋] 
             * …
             * 10: IL_0016: stloc.1　<=ここで判断処理後の結果がローカル1にセットされて、次のldloc.1で読み込まれてるが、読み込まれる前に値を変えてやりたいというわけ。
             * 11: IL_0017: ldloc.1
             * 12: IL_0018: brfalse   IL_03E4
             * だからその前に、値を変える処理を差し込む場所を見つけ出し、posに保存したのがここまでの処理。
             */

            /*
             * 今までのコードは、ただ処理を挿入したい場所を特定していただけだ。ここからが実際の処理だ。
             * 無理矢理！Trueの値を返す処理changeResultを使って、武器の所持判定など無視して値をTrueにしてやれ。
             */
            if (pos > 0)
            {
                /*
                 * list.InsertRangeを使って、処理を挿入したい場所に新たなCodeInstructionを挿入してやれ。
                 */
                list.InsertRange(pos, new List<CodeInstruction>
                {
                    /*
                     * これからお前は、loc.1にセットされている値を無理矢理！Trueにするわけだ。
                     * だからまずは、OpCodes.Ldlocaで1を指定し、loc.1にTrueを設定出来るようにしてやれ。
                     * ※ ここで使用しているLdlocの後のaはアドレスの意味で、参照を渡している。loc.1の値をいじりたいから今回は必要。
                     */
                    new CodeInstruction(OpCodes.Ldloca, 1),
                    /*
                     * 次はOpCodes.Callを使って、無理矢理！TrueにするchangeResultメソッドを呼び出すんだ。
                     * 呼び出すにはAccessTools.Methodを使い、クラス（今回はVerb_TigerStrike_Patchクラス）とメソッド（changeResult）を指定
                     * しろ。
                     */
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Verb_TigerStrike_Patch), nameof(changeResult), null, null))
                    //new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Verb_TigerStrike_Patch), "changeResult", null, null))
                });
            }
            else
            {
                // error
                Log.Warning("ARoM_CustomPatch Transpiler failed!", false);
            }
            Log.Message("ARoM_CustomPatch: \n\n" + string.Join("\n", list.Select(x => x.ToString()).ToArray()) + "\n\n");
            /*
             * これで、TigerStrikeは武器を装備していても使用ができる、従順スキルになるわけだ。
             * さあ、無理矢理！Trueにする命令を挿入したlistを返してやれ。これで作業は完了、あとはRimworldをプレイするだけだ。
             */
            return list;
        }

        // 引数の値をいじる時はrefが必要。つまり参照渡し。
        private static void changeResult(ref bool result)
        {
            result = true;
        }

        /*
         * このHarmonyTranspilerは、ゲームが実行するコード（正確には中間言語らしい？）に対して、 実行させたいコードを追加するためのものの様子。
         * 基本的に、ILコードで処理を挟みたい場所を探し出し、そこに処理を挟んだ一連の処理リスト（上記ではlist）をreturnしてやることで、ゲームが
         * 追加した処理を含めたコードを実行してくれる、といった感じです。(maybe)
         */
    }
}

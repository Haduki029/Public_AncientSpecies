/// <summary>
/// ①まず弄りたい該当部分とその前後のコードを俯瞰します。
/// 
///bool flag = true;
///bool flag2 = base.get_CasterPawn().equipment.get_Primary() == null; ←今回はこれをtrueに上書きする
///bool result;
///if (flag2)
///{ ...
///
/// 
/// ②ILに切替え、対応する部分に目星を付けます。
///
///IL_0000: nop
///IL_0001: ldc.i4.1
///IL_0002: stloc.0
///IL_0003: ldarg.0
///IL_0004: call instance class ['Assembly-CSharp'] Verse.Pawn['Assembly-CSharp'] Verse.Verb::get_CasterPawn()
///IL_0009: ldfld class ['Assembly-CSharp'] Verse.Pawn_EquipmentTracker['Assembly-CSharp'] Verse.Pawn::equipment
///IL_000e: callvirt instance class ['Assembly-CSharp'] Verse.ThingWithComps['Assembly-CSharp'] Verse.Pawn_EquipmentTracker::get_Primary()
///IL_0013: ldnull
///IL_0014: ceq
///IL_0016: stloc.1
///IL_0017: ldloc.1
///IL_0018: brfalse IL_03e4
/// 
/// ③１行単位で対応する部分を分解してみます。
///
/// IL_0000: nop
/// 
/// =====    bool flag = true; 
/// |
/// |   IL_0001: ldc.i4.1   ：評価スタックに(int32)１　(= true) をプッシュ
/// |   IL_0002: stloc.0    ：ローカル０にセット
/// |_
///
/// =====    bool flag2 = base.get_CasterPawn().equipment.get_Primary() == null;
/// |
/// |   IL_0003: ldarg.0    :評価スタックにロード。非staticメソッドのarg0はインスタンス自身(=Verb_TigerStrike)を指す。
/// |   IL_0004: call instance class ['Assembly-CSharp'] Verse.Pawn['Assembly-CSharp'] Verse.Verb::get_CasterPawn() ：以下get_Primary()までVerb_TigerStrikeから順に読み込んでいく。
/// |   IL_0009: ldfld class ['Assembly-CSharp'] Verse.Pawn_EquipmentTracker['Assembly-CSharp'] Verse.Pawn::equipment
/// |   IL_000e: callvirt instance class ['Assembly-CSharp'] Verse.ThingWithComps['Assembly-CSharp'] Verse.Pawn_EquipmentTracker::get_Primary()
/// |   IL_0013: ldnull     ：右辺にnullをロード
/// |   IL_0014: ceq        ：左辺(２個前)と右辺(直前)の評価スタックが等しければ1、それ以外は0を評価スタックにプッシュ（==オペレータ）
/// |   IL_0016: stloc.1    ：ローカル１に上の結果をセット
/// |   
/// |   
/// =====    if (flag2){ ...
/// |
/// |   IL_0017: ldloc.1            ：ローカル１を評価スタックにロード
/// |   IL_0018: brfalse IL_03e4    ：評価スタックが０またはNullなら指定ラベルへジャンプ
/// |
/// ...
/// 
/// ④-0. 挿入方法を考える
/// 　Harmonyのwikiにもあるように、ルール違反状態のコードにならないよう整合性を保たなければいけません。
/// 　上でいえば、ifジャンプ brfalseの前には１つ、関係演算子 ceqの前には２つの評価スタックが不可欠です。
/// 　既存の後続が使う評価スタックを独自処理で消費してしまった場合、必ずメソッドの戻り値か新規ロードで
/// 　代わりの評価スタックを用意する必要がありますし、逆に余らせてもいけません。
/// 　ダメな例１：）ceq直後にtrueをプッシュしてぶっこむ
/// 　      
///         ceq
///         ldc.i4.1 
///         stloc.1     ←stlocは１つの評価スタックしか使わない。評価スタック余りで不整合
///         ldloc.1
///         ...
///         
/// ④-1. 挿入
/// 
/// 
/// </summary>
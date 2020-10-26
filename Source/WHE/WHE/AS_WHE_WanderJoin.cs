using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AS_WHE
{
    public class AS_WHE_WanderJoin : WorldComponent
    {
        /// <summary>
        /// ここはおまじないなので読み飛ばしてOK
        /// </summary>
        /// <param name="world"></param>
        public AS_WHE_WanderJoin(World world) : base(world)
        {
        }
        /// <summary>
        /// ここがゲーム中にTick毎に実行してくれます。
        /// </summary>
        public override void WorldComponentTick()
        {
            base.WorldComponentTick();

            // ここに雪雫が出てくるコードを追加しましょう。

            // 大雪中のプレイヤー勢力のMAPがある場合
            IEnumerable<Map> RandomPlayerMap;
            // Player FactionのMapデータを設定
            RandomPlayerMap =
                from CheckMap in Find.Maps
                where (CheckMap.ParentFaction == Faction.OfPlayer)
                select CheckMap;

            if (RandomPlayerMap.Count() > 0)
            {
                // 大雪のMAPがあった時にここの処理が走ります。
            }
        }
    }
}

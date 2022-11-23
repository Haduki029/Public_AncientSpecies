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
        {}

        /// <summary>
        /// Thing側から自身を所有しているPawnを探し当てるメソッド。見つからなければnullを返すので注意。
        /// </summary>
        public static Pawn GetMyOwner<T>(T thing) where T : Thing
        {
            if (thing == null)
            {
                return null;
            }
            IThingHolder parent = thing.ParentHolder;
            if (parent == null)
            {
                return null;
            }
            else if (parent is Pawn)
            {
                return parent as Pawn;
            }
            Pawn owner = GetParent(parent) as Pawn;//一世代上に見つからなかったので二世代以上上はサブメソッドに丸投げ
            return owner;
        }
        /// <summary>
        /// GetMyOwnerの付録のサブメソッド。Pawnかnullが見つかるまで一つ上の世代を調べ続ける。
        /// </summary>
        private static IThingHolder GetParent(IThingHolder holder)
        {
            IThingHolder parent = holder.ParentHolder;//親
            if (parent == null)
            {
                return null;//行き止まりでした
            }
            if(parent is Pawn)
            {
                return parent;//Pawnを見つけました
            }
            return GetParent(parent);//更に上の世代を探します
        }
    }

}

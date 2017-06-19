using UnityEngine;
using System.Collections;

namespace Komugi
{
    public struct StageData
    {
        /** ステージID */
        public int id;

        /** コメント（リリース時は消す予定） */
        public string comment;

        /** プレファブ名 */
        public string prefab;

        /** 突破するためのギミックID */
        public int gimmickId;

        /** 次の行先 */
        public int nextStage;

        /** 前の場所 */
        public int preiverStage;

        /** ジャンプ先 */
        public int[] jumpToStage;

        /** 入手できるアイテムのID */
        public int[] getItem;
    }
}
using UnityEngine;
using System.Collections;

namespace Komugi
{
    public struct StageData
    {
        /** ステージID */
        public int id;

        /** プレファブ名 */
        public string prefab;

        /** 扉を開くためのトリックタイプ  1 扉  2 パスワード */
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
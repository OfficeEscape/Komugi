namespace Komugi
{
    #region ---------------------- StageData ----------------------
    // ステージデータ
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
    #endregion

    #region ---------------------- ItemData ----------------------
    public struct ItemData
    {
        // アイテムのユニークなID
        public int itemId;

        // アイテム名
        public string itemName;

        // アイテム説明
        public string itemDetail;

        // アイテムアイコンのパス
        public string itemIcon;

        // アイテム画像のパス
        public string itemImage;

        // 変化後アイテム
        public int changeItem;

        // 変化するトリガーとなるアイテム
        public int triggerItem;
    }
    #endregion

    #region ---------------------- GimmickData ----------------------
    public struct GimmickData
    {
        /** コメント */
        public string comment;

        public int gimmickId;

        /** ギミックタイプ 1 アイテム 2 パスワード */
        public int gimmickType;

        /** ギミック解くための正解 */
        public int gimmickAnswer;
    }
    #endregion
}

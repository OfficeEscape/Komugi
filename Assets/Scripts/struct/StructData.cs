namespace Komugi
{
    public struct UserData
    {
        /** 最後に止まったステージのID */
        public int currentStageId;

        /** 所持アメ数 */
        public int candyNum;
        
        public UserData(int sid = 1, int cn = 0)
        {
            currentStageId = sid;
            candyNum = cn;
        }
    }

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

        // アイテム自動変化するか
        public int autoChange;
    }
    #endregion

    #region ---------------------- GimmickData ----------------------
    public struct GimmickData
    {
        /** コメント */
        public string comment;

        public int gimmickId;
        
        /** クリア後別のステージへ飛ぶか 0 だと飛ばない それ以外は飛び先を指す */
        public int clearJump;

        /** ギミック解くための正解 */
        public int[] gimmickAnswer;

        /** クリア時に再生するSE */
        public string clearSe;

        /** クリア時の文言 */
        public string clearMessage;
    }
    #endregion

    #region ---------------------- HintData ----------------------
    public struct HintData
    {
        public int hintId;

        /** ヒントタイトル */
        public string title;

        /** ヒント詳細 */
        public string detail;

        public int gimmickId;

        /** 解放に必要なアメの数 */
        public int candyNum;
    }
    #endregion
}

using UnityEngine;
using System.Collections;

public class StageData {

    /** ステージID */
    public int id;

    /** プレファブ名 */
    public string prefab;

    /** ステージタイプ 1 最初からいける 0 他のステージ経由で行ける */
    public int stageType;

    /** 扉がある場合の開閉画像 */
    public string[] door;

    /** 扉を開くためのトリックタイプ */
    public string openType;

    /** 使うアイテムのID */
    public int itemId;

    /** 次の行先 */
    public int nextStage;

	/** 前の場所 */
	public int preiverStage;

	/** ジャンプ先 */
	public int jumpToStage;

    /** 入手できるアイテムのID */
    public int getItem;

    /** 画像の変更がある場合の画像の配列 */
    public string[] changeBg;
}

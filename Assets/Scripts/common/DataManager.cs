using Komugi.Data;
using System.Collections.Generic;

namespace Komugi
{
    public class DataManager
    {
        #region ============= シングルトン =============
        /** クラスのユニークなインスタンス */
        private static DataManager mInstance;

        public static DataManager Instance
        {
            get
            {
                if (mInstance == null) mInstance = new DataManager();

                return mInstance;
            }
        }

        #endregion
        
        private static readonly string ITEM_DATA_KEY = "ItemSaveData";

        private static readonly string GIMMICK_DATA_KEY = "GimmickSaveData";

        /// <summary>
        /// データ保存
        /// </summary>
        public void SaveData()
        {
            PlayerPrefsUtility.SaveDict(ITEM_DATA_KEY, ItemManager.Instance.HasItemList);
            PlayerPrefsUtility.SaveDict(GIMMICK_DATA_KEY, GimmickManager.Instance.clearFlagDictionary);
        }

        public void SaveDataReset()
        {
            UnityEngine.PlayerPrefs.DeleteKey(ITEM_DATA_KEY);
            UnityEngine.PlayerPrefs.DeleteKey(GIMMICK_DATA_KEY);
        }

        /// <summary>
        ///  ギミックデータロード
        /// </summary>
        /// <returns>セーブデータ</returns>
        public Dictionary<int, int> LoadGimmickSaveData()
        {
            return PlayerPrefsUtility.LoadDict<int, int>(GIMMICK_DATA_KEY);
        }

        /// <summary>
        /// アイテムデータロード
        /// </summary>
        /// <returns>セーブデータ</returns>
        public Dictionary<int, bool> LoadItemSaveData()
        {
            return PlayerPrefsUtility.LoadDict<int, bool>(ITEM_DATA_KEY);
        }
    }
}
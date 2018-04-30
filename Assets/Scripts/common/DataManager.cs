using Komugi.Data;
using UnityEngine;
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

        private static readonly string BGM_KEY = "Bgm";

        private static readonly string SE_KEY = "Se";

        /// <summary>
        /// データ保存
        /// </summary>
        public void SaveData()
        {
            PlayerPrefsUtility.SaveDict(ITEM_DATA_KEY, ItemManager.Instance.HasItemList);
            PlayerPrefsUtility.SaveDict(GIMMICK_DATA_KEY, GimmickManager.Instance.clearFlagDictionary);
        }

        public void SaveOption(int bgm, int se)
        {
            PlayerPrefs.SetInt(BGM_KEY, bgm);
            PlayerPrefs.SetInt(SE_KEY, se);
        }

        public void SaveDataReset()
        {
            PlayerPrefs.DeleteKey(ITEM_DATA_KEY);
            PlayerPrefs.DeleteKey(GIMMICK_DATA_KEY);
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

        public int LoadBGMOption()
        {
            return PlayerPrefs.GetInt(BGM_KEY);
        }

        public int LoadSEOption()
        {
            return PlayerPrefs.GetInt(SE_KEY);
        }
    }
}
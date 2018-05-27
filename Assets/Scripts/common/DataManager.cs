using Komugi.Data;
using LitJson;
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

        private static readonly string HINT_DATA_KEY = "HintSaveData";

        private static readonly string BGM_KEY = "Bgm";

        private static readonly string SE_KEY = "Se";

        private static readonly string USER_KEY = "user";

        private UserData userData;

        public UserData UserSaveData { get { return userData; } }
        
        private DataManager()
        {
            userData = new UserData(1, 0);
        }

        #region ---------------------- セーブ ----------------------
        /// <summary>
        /// データ保存
        /// </summary>
        public void SaveData()
        {
            PlayerPrefsUtility.SaveDict(ITEM_DATA_KEY, ItemManager.Instance.HasItemList);
            PlayerPrefsUtility.SaveDict(GIMMICK_DATA_KEY, GimmickManager.Instance.clearFlagDictionary);
        }

        public void SaveHintData()
        {
            PlayerPrefsUtility.SaveDict(HINT_DATA_KEY, GimmickManager.Instance.hintPayDictionary);
        }

        public void SaveOption(int bgm, int se)
        {
            PlayerPrefs.SetInt(BGM_KEY, bgm);
            PlayerPrefs.SetInt(SE_KEY, se);
        }

        public void SaveUserData()
        {
            PlayerPrefs.SetString(USER_KEY, JsonMapper.ToJson(userData));
        }
        
        #endregion

        #region ---------------------- ロード ----------------------
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

        public Dictionary<int, bool> LoadHintSaveData()
        {
            return PlayerPrefsUtility.LoadDict<int, bool>(HINT_DATA_KEY);
        }

        public int LoadBGMOption()
        {
            return PlayerPrefs.GetInt(BGM_KEY);
        }

        public int LoadSEOption()
        {
            return PlayerPrefs.GetInt(SE_KEY);
        }

        public void LoadUserData()
        {
            if (PlayerPrefs.HasKey(USER_KEY))
            {
                userData = JsonMapper.ToObject<UserData>(PlayerPrefs.GetString(USER_KEY));
            }
        }

        #endregion

        public bool CheckSaveData()
        {
            return PlayerPrefs.HasKey(USER_KEY);
        }

        public void SaveDataReset()
        {
            PlayerPrefs.DeleteKey(ITEM_DATA_KEY);
            PlayerPrefs.DeleteKey(GIMMICK_DATA_KEY);
            PlayerPrefs.DeleteKey(USER_KEY);
            PlayerPrefs.DeleteKey(HINT_DATA_KEY);
        }

        public void AddCandy(int add)
        {
            userData.candyNum += add;
            SaveUserData();
        }

        public void SetStageId(int newid)
        {
            userData.currentStageId = newid;
        }
    }
}
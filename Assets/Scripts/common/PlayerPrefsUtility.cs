using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


namespace Komugi.Data
{
    /// <summary>
    /// PlayerPrefsに関する便利クラス
    /// </summary>
    public class PlayerPrefsUtility
    {

        #region ======== 保存 ========

        /// <summary>
        /// リストを保存
        /// </summary>
        public static void SaveList<T>(string key, List<T> value)
        {
            string serizlizedList = Serialize(value);
            PlayerPrefs.SetString(key, serizlizedList);
        }

        /// <summary>
        /// ディクショナリーを保存
        /// </summary>
        public static void SaveDict<Key, Value>(string key, Dictionary<Key, Value> value)
        {
            string serizlizedDict = Serialize(value);
            PlayerPrefs.SetString(key, serizlizedDict);
        }
        #endregion

        #region ======== 読み込み ========

        /// <summary>
        /// リストを読み込み
        /// </summary>
        public static List<T> LoadList<T>(string key)
        {
            //keyがある時だけ読み込む
            if (PlayerPrefs.HasKey(key))
            {
                string serizlizedList = PlayerPrefs.GetString(key);
                return Deserialize<List<T>>(serizlizedList);
            }

            return new List<T>();
        }

        /// <summary>
        /// ディクショナリーを読み込み
        /// </summary>
        public static Dictionary<Key, Value> LoadDict<Key, Value>(string key)
        {
            //keyがある時だけ読み込む
            if (PlayerPrefs.HasKey(key))
            {
                string serizlizedDict = PlayerPrefs.GetString(key);
                return Deserialize<Dictionary<Key, Value>>(serizlizedDict);
            }

            return new Dictionary<Key, Value>();
        }
        #endregion

        #region ======== シリアライズ、デシリアライズ ========

        private static string Serialize<T>(T obj)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            return Convert.ToBase64String(memoryStream.GetBuffer());
        }

        private static T Deserialize<T>(string str)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
            return (T)binaryFormatter.Deserialize(memoryStream);
        }
        #endregion
    }
}
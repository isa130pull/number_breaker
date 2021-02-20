using System.Collections.Generic;
using Common.Isao;
using UnityEngine;

namespace Isao.Common
{
    /// <summary>
    /// PleyerPrefsのラッパークラス
    /// </summary>
    public static class PlayerPrefsUtility
    {
        /// <summary>
        /// bool値を読込
        /// </summary>
        /// <returns>標準ではPlayerPrefsにbool値を保存できないので、int値で設定する。 (<c>1</c>:true, <c>0</c>:false)</returns>
        public static bool GetBool(string key, bool defaultValue)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key) == 1;
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// bool値を保存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state">標準ではPlayerPrefsにbool値を保存できないので、int値で設定する。 (<c>1</c>:true, <c>0</c>:false)</param>
        public static void SetBool(string key, bool state)
        {
            PlayerPrefs.SetInt(key, state ? 1 : 0);
        }

        // =================================================================================
        // 保存
        // =================================================================================

        /// <summary>
        /// リストを保存
        /// </summary>
        public static void SetList<T>(string key, List<T> value)
        {
            var serializedList = PlayerPrefsUtility.Serialize(new Serialization<T>(value));
            PlayerPrefs.SetString(key, serializedList);
        }

        /// <summary>
        /// ディクショナリーを保存
        /// </summary>
        public static void SetDictionary<Key, Value>(string key, Dictionary<Key, Value> value)
        {
            var serializedDictionary = PlayerPrefsUtility.Serialize(new Serialization<Key, Value>(value));
            PlayerPrefs.SetString(key, serializedDictionary);
        }

        /// <summary>
        /// 指定されたオブジェクトの情報を保存します
        /// </summary>
        public static void SetObject<T>(string key, T obj)
        {
            var json = JsonUtility.ToJson(obj);
            PlayerPrefs.SetString(key, json);
        }

        // =================================================================================
        // 読み込み
        // =================================================================================

        /// <summary>
        /// リストを読み込み
        /// </summary>
        public static List<T> GetList<T>(string key)
        {
            // keyがある時だけ読み込む
            if (!PlayerPrefs.HasKey(key))
            {
                return new List<T>();
            }

            var serializedList = PlayerPrefs.GetString(key);
            return PlayerPrefsUtility.Deserialize<Serialization<T>>(serializedList).ToList();
        }

        /// <summary>
        /// ディクショナリーを読み込み
        /// </summary>
        public static Dictionary<Key, Value> GetDictionary<Key, Value>(string key)
        {
            // keyがある時だけ読み込む
            if (!PlayerPrefs.HasKey(key))
            {
                return new Dictionary<Key, Value>();
            }

            var serializedDictionary = PlayerPrefs.GetString(key);
            return PlayerPrefsUtility.Deserialize<Serialization<Key, Value>>(serializedDictionary).ToDictionary();
        }

        /// <summary>
        /// 指定されたオブジェクトの情報を読み込みます
        /// </summary>
        public static T GetObject<T>(string key)
        {
            var json = PlayerPrefs.GetString(key);
            var obj = JsonUtility.FromJson<T>(json);
            return obj;
        }

        // =================================================================================
        // シリアライズ、デシリアライズ
        // =================================================================================

        private static string Serialize(object obj)
        {
            return JsonUtility.ToJson(obj);
        }

        private static T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}
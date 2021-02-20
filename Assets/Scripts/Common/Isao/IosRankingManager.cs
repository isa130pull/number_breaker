using System;
using Isao.Common;
using UnityEngine;

namespace Common.Isao
{
    public static class IosRankingManager
    {
        public static void Auth(Action<bool> callBack = null)
        {
            if (Utility.IsEditor()) return;
        #if UNITY_IOS
            // コールバックが設定されていない場合はログを設定
            if (callBack == null)
            {
                callBack = success =>
                {
                    if (success) IosRankingManager.GetRanking();
                };
            }

            Social.localUser.Authenticate(callBack);
            // アチーブメント獲得時の通知をONにする
            UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
        #endif
        }

        public static void ShowBoard()
        {
            if (Utility.IsEditor())
            {
                return;
            }
        #if UNITY_IOS
            Social.ShowLeaderboardUI();
        #endif
        }

        public static void ReportScore(long score, string id)
        {
            if (Utility.IsEditor() || Utility.IsAndroid()) return;

        #if UNITY_IOS
            Social.ReportScore(score, id, success =>
            {
                if (success) IosRankingManager.GetRanking();
            });
        #endif
        }

        private static void GetRanking()
        {
            if (Utility.IsEditor() || Utility.IsAndroid()) return;

        #if UNITY_IOS
            var leaderboard = Social.CreateLeaderboard();
            leaderboard.id = PlayerPrefsUtility.GetBool(PrefsKey.IsRankingPlayCount, false)
                ? "breakhouse_playcount_ranking"
                : "breakhouse_money_ranking";
            leaderboard.LoadScores(result =>
            {
                var rank = leaderboard.localUserScore.rank;
                if (rank >= 0)
                {
                    PlayerPrefs.SetInt(PrefsKey.GameCenterRank, rank);
                }
            });
        #endif
        }


        public static void ReportAchievement(string key, float percent = 100f, Action<bool> callBack = null)
        {
            if (Utility.IsEditor())
            {
                return;
            }
        #if UNITY_IOS
            // アチーブメントのインスタンスを作成し、keyと進捗率を設定
            var achievement = Social.CreateAchievement();
            achievement.id = key;
            achievement.percentCompleted = percent;

            // コールバックが設定されていない場合はログを設定
            if (callBack == null)
            {
                callBack = success => { };
            }

            // 送信
            achievement.ReportProgress(callBack);
        #endif
        }

        public static void ResetAchievement()
        {
            if (Utility.IsEditor())
            {
                return;
            }
        #if UNITY_IOS
            UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ResetAllAchievements(null);
        #endif
        }
    }
}
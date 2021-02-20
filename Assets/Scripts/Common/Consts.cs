using System.Collections.Generic;

public static class Enums
{
    public enum DialogType
    {
        OK,
        YesNo,
    }
}

public static class Consts
{
    public static readonly Dictionary<string, object> FBConfigParameter = new Dictionary<string, object>
    {
        {
            FBConfigKey.InterstitialNum, FBConfigValue.InterstitialNum
        },
        {
            FBConfigKey.MovieAdNum, FBConfigValue.MovieAdNum
        },
    };
}

public static class SoundName
{
    public const string NextStage2Se = "nextstage2";
}

public static class FBConfigKey
{
    public const string InterstitialNum = "InterstitialNum";
    public const string MovieAdNum = "MovieAdNum";
}

public static class FBConfigValue
{
    public const int InterstitialNum = 4;
    public const int MovieAdNum = 3;
}


public static class PrefsKey
{
    public const string PlayCount = "PlayCount";
    public const string HighScore = "HighScore";
    public const string HighStage = "HighStage";
    public const string CurrentPoint = "CurrentPoint";
    public const string GameCenterRank = "GameCenterRank";
    public const string ShowTutorialText = "ShowTutorialText";
    public const string CurrentMovieAdCount = "CurrentMovieAdCount";
    public const string IsShowNextStage = "IsShowNextStage";
    public const string IsPlayTutorialEnd = "IsPlayTutorialEnd";
    public const string Is60Fps = "Is60Fps";

    public const string AttackLevel = "AttackLevel";
    public const string SizeLevel = "SizeLevel";
    public const string BounceLevel = "BounceLevel";
    public const string WaitShotLevel = "WaitShotLevel";
    public const string SushiLevel = "SushiLevel";

    public const string IsMuteBgm = "IsMuteBgm";
    public const string IsMuteSe = "IsMuteSe";
    public const string IsJapanese = "IsJapanese";

    public const string IsRankingPlayCount = "IsRankingPlayCount";
}

public static class PrefsDefaultValue
{
    public const int AttackLevel = 1;
    public const int SizeLevel = 1;
    public const int BounceLevel = 1;
    public const int SushiLevel = 1;
    public const int WaitShotLevel = 1;
    public const int PlayCount = 0;
}
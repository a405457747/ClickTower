using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace xmaolol.com
{
    public static class Consts
    {


        #region 声音名字部分
        public const string enterEffect = "BuffBtnClick";
        public const string backEffect = "Cursor_002";
        public const string selectTower = "TDSelect";
        public const string BuildTower = "ui_sound_forward";
        public const string SellTower = "TD Tower Sell";
        public const string coinEffect = "Coins_Few_03";
        public const string heartEffect = "zone_enter";
        public const string WinEffect = "TD Victory";
        public const string LoseEffect = "TD Defeat";
        public const string humanDie = "aargh0";
        public const string robotDie = "Enemies Exploding 3";
        public const string bulletHit = "shot_hand_gun";
        public const string BoomHit = "explosion_bazooka";
        #endregion

        public const float AnimationResetTime = 0.000001f;
        //过度效果持续时间
        public const float MaskPanelSaveTime = 0.5f;
        //也就是20个关卡
        public const int MaxGameLevel = 39;
        //打开或关闭Panel的延迟时间
        public const float PanelDelayTime = 1f;
        //打开或关闭Panel这个进程的时间
        public const float PanelProcessTime = 1f;

        public const int TowerMaxLevel = 3;
        public const float AddExplodeRate = 0.13f;
        public const int maxDemage = 9999999;
        //致死率
        public const float fatalityRate = 0.23f;
        //t1的百分比伤害值
        public const float percentageDemageT1 = 0.11f;
        //灼烧持续时间3f;
        public const float DurationOfBurningSaveTime = 8f;
        //灼烧伤害量是当前生命值百分比
        public const float DurationOfBurningDemagePercentage = 0.35f;
        public const float DurationOfBurningBrithRate = 0.75f;
        //每级价格增长倍数
        public const float PerLevelPriceAddRate = 1f;
        internal static float ComLogoSaveTime = 3.4f;
        internal static float ComLogoFadeTime = 0.5f;
    }

    public enum EnemyDir
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum GameState
    {
        Null,
        GameStart,
        Pause,
        LevelAdd,
        GameOver,
        Win
    }

    public enum Morale
    {
        White,
        Red,
        yellow,
        Blue
    }
}


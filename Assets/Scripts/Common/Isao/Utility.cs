using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Isao.Common;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
#if UNITY_IOS
using UnityEngine.iOS;

#endif


namespace Common.Isao
{
    public static class Utility
    {
        public static bool IsIOS()
        {
            return Application.platform == RuntimePlatform.IPhonePlayer;
        }

        public static bool IsAndroid()
        {
            return Application.platform == RuntimePlatform.Android;
        }

        public static bool IsIphoneX()
        {
            return (float) Screen.width / Screen.height <= 0.47;
        }

        public static bool IsIPad()
        {
            return (float) Screen.width / Screen.height >= 0.7;
        }

        public static bool IsEditor()
        {
        #if UNITY_EDITOR
            return true;
        #endif
            return false;
        }

        public static void IosReviewDialog()
        {
            //iOSの時だけアプリ内レビュー
        #if UNITY_IOS
            Device.RequestStoreReview();
        #endif
        }

        public static string GetGooglePlayStoreUrl()
        {
            return $"https://play.google.com/store/apps/details?id={Application.identifier}";
        }

        public static bool IsNetworkConnection()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        /// Spriteデータを返す
        public static Sprite CreateSprite(string filePath)
        {
            var texture = Resources.Load<Texture2D>(filePath);
            if (texture == null)
            {
                Debug.LogError(filePath + "のパスにTexture2Dに変換できるファイルは見つかりません");
                return null;
            }

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        public static Button TouchButton(Button button, string tapImageName = "", VibrationType vibrationType = VibrationType.None,
            float expansionRate = 1.2f)
        {
            // ボタン押下時の色変更を消す
            var buttonColors = button.colors;
            buttonColors.pressedColor = Color.white;
            buttonColors.highlightedColor = Color.white;
            button.colors = buttonColors;

            var tapBeforeImage = button.image.sprite;
            var baseRate = button.transform.localScale;

            button.OnPointerDownAsObservable()
                .TakeUntilDestroy(button)
                .Where(_ => button.interactable)
                .Subscribe(__ =>
                {
                    // タップ中の画像差し替え
                    if (tapImageName != "")
                    {
                        button.image.sprite = Utility.CreateSprite(tapImageName);
                    }

                    // バイブレーション
                    switch (vibrationType)
                    {
                        case VibrationType.Weak:
                            Vibration.WeakVibration();
                            break;
                        case VibrationType.Middle:
                            Vibration.MiddleVibration();
                            break;
                        case VibrationType.Strong:
                            Vibration.StrongVibration();
                            break;
                    }

                    button.transform.DOScale(new Vector3(baseRate.x * expansionRate, baseRate.y * expansionRate), 0.1f);
                });

            button.OnPointerUpAsObservable()
                .TakeUntilDestroy(button)
                .Subscribe(_ =>
                {
                    button.transform.DOScale(baseRate, 0.1f);
                    // 画像を元に戻す
                    if (tapImageName != "")
                    {
                        button.image.sprite = tapBeforeImage;
                    }
                });

            return button;
        }

        /// <summary>
        /// uGUIの拡大縮小を繰り返すアニメーション作成
        /// </summary>
        public static void ZoomInOutAnimationStart(Transform transform)
        {
            DOTween.Sequence()
                .AppendInterval(0.2f)
                .Append(transform.DOScale(1.1f, 0.6f).SetEase(Ease.OutQuad))
                .AppendInterval(0.05f)
                .Append(transform.DOScale(1.0f, 0.6f).SetEase(Ease.OutQuad))
                .SetLoops(-1)
                .Play();
        }

        /// <summary>
        /// 重複なしのランダムな値を返す
        /// </summary>
        public static int[] GetRandomToNotDuplicate(int from, int to, int count)
        {
            var notDuplicates = new List<int>();

            for (var i = 0; i < count;)
            {
                var param = Random.Range(from, to);
                if (notDuplicates.Contains(param))
                {
                    continue;
                }

                notDuplicates.Add(param);
                i++;
            }

            return notDuplicates.ToArray();
        }

        /// <summary>
        /// ランダムな数値を返す、除外する数値も設定できる
        /// </summary>
        public static int ExclusionRandom(int from, int to, int exclusion)
        {
            for (;;)
            {
                var param = Random.Range(from, to);
                if (param != exclusion)
                {
                    return param;
                }
            }
        }

        public static bool IsTrueOrFalse()
        {
            return Random.Range(0, 2) == 1;
        }

        public static Vector3 GetTabletCanvasScale()
        {
            var screenRate = (float) Screen.width / Screen.height;
            const float baseRate = 0.5625f;
            // 9/16より縦長の端末では補正をしない
            if (screenRate <= baseRate)
            {
                return Vector3.one;
            }

            // iPad(縦横比率0.7)の時にScaleが0.85になるように線形的に計算
            var fixRate = 1 - ((screenRate - baseRate) / 0.1875f * 0.15f);
            fixRate = Mathf.Max(fixRate, 0.85f);
            return new Vector3(fixRate, fixRate, fixRate);
        }

        /// <summary>
        /// 三平方の定理で距離を求める
        /// </summary>
        public static float GetDistance(Vector3 sourcePosition, Vector3 targetPosition)
        {
            return Mathf.Sqrt(Mathf.Pow(sourcePosition.x - targetPosition.x, 2) + Mathf.Pow(sourcePosition.y - targetPosition.y, 2) +
                              Mathf.Pow(sourcePosition.z - targetPosition.z, 2));
        }
        
        public static float ToRadian(float angle)
        {
            return angle * Mathf.PI / 180;
        }
        
    }
}
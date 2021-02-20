using System.Runtime.InteropServices;

namespace Isao.Common
{
    public enum VibrationType
    {
        None,
        Weak,
        Middle,
        Strong,
    }

    public static class Vibration
    {
    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void WeakVibration_();

        [DllImport("__Internal")]
        private static extern void MiddleVibration_();

        [DllImport("__Internal")]
        private static extern void StrongVibration_();
    #endif

        public static void WeakVibration()
        {
        #if UNITY_IOS && !UNITY_EDITOR
            WeakVibration_();
        #endif
        }

        public static void MiddleVibration()
        {
        #if UNITY_IOS && !UNITY_EDITOR
            MiddleVibration_();
        #endif
        }

        public static void StrongVibration()
        {
        #if UNITY_IOS && !UNITY_EDITOR
            StrongVibration_();
        #endif
        }
    }
}
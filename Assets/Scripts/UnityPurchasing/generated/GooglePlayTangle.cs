#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("XZ470Hvpi9FZdx91jixLeaJNfdVl5ujn12Xm7eVl5ubnOqp9DOS7L5hSmZsR7zcHkOzY6g5zcks6nZNA01N1dlNrNHKxvIrs1ChOx7EdiwYteIIkDmL9mbT+XBhBOi31XEq13Oyc/YavBlc5vdvGtFwYRWucy+9RwKfyiY79MGKE9j8yLHMTFfGMVQOODiHwMzyy5l7JFGxuqUqhoUofhi3jjmCqWAVR1kt6GxjDnNfWDWsIhKP15toLEuszoWQ2kprpc6R0M1zoEPPjiewaHN4rKPR+1AwAQLhYANdl5sXX6uHuzWGvYRDq5ubm4ufk+CD2mbAh/bMUlmxaAHANDdrSFzHAE8Ulo5Ll6TjGyFYlM2TAEQg60QOfIuC253XEYOXk5ufm");
        private static int[] order = new int[] { 10,1,11,4,9,7,8,9,8,12,13,13,12,13,14 };
        private static int key = 231;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif

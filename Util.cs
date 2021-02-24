namespace CropReplant
{
    static class Util
    {
        public static void DBG(string str = "")
        {
            UnityEngine.Debug.Log((typeof(CropReplant).Namespace + " ") + str);
        }
    }
}

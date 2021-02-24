using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

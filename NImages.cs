using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neko11V2
{
    static class NImages
    {
        public static Dictionary<string, Image> LoadImages()
        {
            Dictionary<string, Image> result = [];
            string[] files = Directory.GetFiles("img/");

            foreach (string file in files)
            {
                if (file.EndsWith(".ico"))
                {
                    result.Add(file.Replace("img/", "").ToLower(), Image.FromFile(file));
                }
            }

            return result;
        }
    }
}

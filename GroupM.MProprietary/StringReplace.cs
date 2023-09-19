using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MProprietary
{
    public class StringReplace
    {
        public string ReplaceSpecialCharacter(string specialText)
        {
            string text = specialText.Replace("!", "")
                          .Replace("@", "")
                          .Replace("#", "")
                          .Replace("$", "")
                          .Replace("%", "")
                          .Replace("^", "")
                          .Replace("&", "")
                          .Replace("*", "")
                          .Replace("=", "")
                          .Replace("[", "")
                          .Replace("]", "")
                          .Replace("'", "");
            return text;
        }
    }
}

using System.Text.RegularExpressions;

namespace Html2UnityRich
{
    public static class Html2UnityRichMgr
    {
        public static void Html2UnityRichText (string str)
        {

        }

        /// <summary>
        /// 创建一个Html根节点
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static HtmlNode CreateHtmlRootNode (string str)
        {
            str = Regex.Replace (str , "<head>.*?</head>" , string.Empty , RegexOptions.Singleline);
            char [] charArray = str.ToCharArray ();
            HtmlMachine machine = new HtmlMachine ();
            for ( int i = 0 ; i < charArray.Length ; i++ )
            {
                machine.CurrentStatus.ApendChar (charArray [i]);
            }
            machine.CurrentStatus.EndApend ();
            return machine.htmlTagAnalyer.root;
        }

        /// <summary>
        /// 是否为单标签
        /// </summary>
        /// <param name="tagStartName"></param>
        /// <returns></returns>
        public static bool IsSingleTag (string tagStartName)
        {
            return tagStartName == HtmlTagName.HTML_TAG_BR;
        }
    }
}
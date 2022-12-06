using System.Collections.Generic;

namespace Html4UnityText
{
    public sealed class HtmlTagAnalyer
    {
        public readonly HtmlNode root;
        private readonly Stack<HtmlNode> _parentHtmlNodeStack;

        public HtmlTagAnalyer ()
        {
            root = new HtmlTagNode (tagStartName: "" , tagEndName: "");
            _parentHtmlNodeStack = new Stack<HtmlNode> ();
            _parentHtmlNodeStack.Push (root);
        }

        /// <summary>
        /// 添加一个纯文本节点
        /// </summary>
        /// <param name="text"></param>
        public void AddTextNode (string text)
        {
            if ( string.IsNullOrEmpty (text) )
            {
                return;
            }
            _parentHtmlNodeStack.Peek ().GetChilds ().Add (new HtmlTextNode (text));
        }

        /// <summary>
        /// 添加一个标签节点
        /// </summary>
        /// <param name="str"></param>
        public void AddTagNode (string str)
        {
            var tagNode = new HtmlTagNode (tagStartName: str , tagEndName: str);
            _parentHtmlNodeStack.Peek ().GetChilds ().Add (tagNode);
            _parentHtmlNodeStack.Push (tagNode); //作为下一个节点的父节点
        }

        /// <summary>
        /// 添加一个单标签节点
        /// </summary>
        /// <param name="str"></param>
        public void AddSingleNode (string str)
        {
            _parentHtmlNodeStack.Peek ().GetChilds ().Add (new HtmlTagNode (tagStartName: str , tagEndName: str));
        }

        /// <summary>
        /// 结束添加子节点
        /// </summary>
        /// <param name="endTagName"></param>
        public void AppendEnd (string endTagName)
        {
            _parentHtmlNodeStack.Pop ();
        }
    }
}
using System.Collections.Generic;

namespace Html2UnityRich
{
    public class HtmlTextNode : HtmlNode
    {
        public readonly string text;

        public HtmlTextNode (string text , int depth)
        {
            this.text = text;
            this.depth = depth;
        }

        public override HtmlNode ToPropNode ()
        {
            return this;
        }

        public override HtmlNode ToUnityRichNode ()
        {
            return this;
        }

        public override Dictionary<string , string> GetProp ()
        {
            return new Dictionary<string , string> (0);
        }

        public override List<HtmlNode> GetChilds ()
        {
            return new List<HtmlNode> (0);
        }

        public override string ToUguiRichText ()
        {
            return text;
        }

        public override string ToTextProRichText ()
        {
            return text;
        }
    }
}
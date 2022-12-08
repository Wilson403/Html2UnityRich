using System.Collections.Generic;

namespace Html4UnityText
{
    public class HtmlTextNode : HtmlNode
    {
        public readonly string text;

        public HtmlTextNode (string text)
        {
            this.text = text;
        }

        public override HtmlNode ToPropNode ()
        {
            return this;
        }

        public override HtmlNode ToUnityRichNode ()
        {
            return this;
        }

        public override List<HtmlNode> GetChilds ()
        {
            return new List<HtmlNode> (0);
        }

        public override string ToUnityRichText ()
        {
            return text;
        }
    }
}
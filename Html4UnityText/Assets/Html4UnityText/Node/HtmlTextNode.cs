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
    }
}
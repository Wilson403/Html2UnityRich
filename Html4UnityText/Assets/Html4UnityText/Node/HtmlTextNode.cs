namespace Html4UnityText
{
    public class HtmlTextNode : HtmlNode
    {
        public readonly string text;

        public HtmlTextNode (string text)
        {
            this.text = text;
        }
    }
}
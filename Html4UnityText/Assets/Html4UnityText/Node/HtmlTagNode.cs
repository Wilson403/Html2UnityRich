namespace Html4UnityText
{
    public class HtmlTagNode : HtmlNode
    {
        public readonly string tagStartName;
        public readonly string tagEndName;

        public HtmlTagNode (string tagStartName , string tagEndName)
        {
            this.tagStartName = tagStartName;
            this.tagEndName = tagEndName;
        }
    }
}
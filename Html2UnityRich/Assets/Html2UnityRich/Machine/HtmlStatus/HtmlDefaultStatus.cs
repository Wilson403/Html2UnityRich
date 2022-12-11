namespace Html2UnityRich
{
    public class HtmlDefaultStatus : HtmlStatus
    {
        public HtmlDefaultStatus (HtmlMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                //遇到了标签起始位，先合并之前添加的字符，再交由HtmlStartOrEndStatus来判断标签含义
                case '<':
                    machine.htmlTagAnalyer.AddTextNode (machine.MergeChar ());
                    machine.AddChar (content);
                    machine.EnterStats (machine.htmlStartOrEndStatus);
                    break;

                default:
                    machine.AddChar (content);
                    break;
            }
        }
    }
}
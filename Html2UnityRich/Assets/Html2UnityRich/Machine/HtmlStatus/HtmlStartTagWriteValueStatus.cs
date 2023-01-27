namespace Html2UnityRich
{
    //原封不动加入字符，不对任何字符做分析，这就是该状态的职责
    public class HtmlStartTagWriteValueStatus : HtmlStatus
    {
        public HtmlStartTagWriteValueStatus (HtmlMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            machine.AddChar (content);
            switch ( content )
            {
                //值写入完成了，交给htmlStartTagStatus继续分析标签状态
                case '"':
                    machine.EnterStats (machine.htmlStartTagStatus);
                    break;
            }
        }
    }
}
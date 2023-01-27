namespace Html2UnityRich
{
    public class HtmlStartTagStatus : HtmlStatus
    {
        public HtmlStartTagStatus (HtmlMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            machine.AddChar (content);
            switch ( content )
            {
                //在起始标签位里遇到了‘/’，说明这是一个单标签，走单标签状态
                case '/':
                    machine.EnterStats (machine.htmlSingleTagStatus);
                    break;

                //起始标签的结束，创建一个新的标签节点后回到默认状态
                case '>':
                    machine.htmlTagAnalyer.AddTagNode (machine.MergeChar ());
                    machine.EnterStats (machine.htmlDefaultStatus);
                    break;

                //起始标签中遇到"，说明正在写入值，走赋值状态处理
                case '"':
                    machine.EnterStats (machine.htmlStartTagWriteValueStatus);
                    break;
            }
        }
    }
}
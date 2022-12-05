namespace Html4UnityText
{
    /// <summary>
    /// 检测该标签是开始还是结束
    /// </summary>
    public class HtmlStartOrEndStatus : HtmlStatus
    {
        public HtmlStartOrEndStatus (HtmlMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            machine.AddChar (content);

            //根据输入的第一个字符来决定下一步的状态
            switch ( content )
            {
                //如果首次得到的输入是‘/’，那么就是结束标签，接下来交由HtmlEndTagStatus处理
                case '/':
                    machine.EnterStats (machine.htmlEndTagStatus);
                    break;

                //其他字符，至少不会是结束标签了，交由HtmlStartTagStatus判断该标签的具体含义
                default:
                    machine.EnterStats (machine.htmlStartTagStatus);
                    break;
            }
        }
    }
}
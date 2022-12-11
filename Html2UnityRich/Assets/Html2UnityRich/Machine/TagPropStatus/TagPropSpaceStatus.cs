namespace Html2UnityRich
{
    public class TagPropSpaceStatus : TagPropStatus
    {
        public TagPropSpaceStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                //忽略多余的空格
                case ' ':
                    break;

                //空格之后还是遇到了结束字符，进结束状态
                case '/':
                case '>':
                    machine.EnterStats (machine.tagPropEndStatus);
                    break;

                default:
                    machine.AddChar (content);
                    machine.EnterStats (machine.tagPropKeyStatus);
                    break;
            }
        }
    }
}
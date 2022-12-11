namespace Html2UnityRich
{
    public class TagPropKeyStatus : TagPropStatus
    {
        public TagPropKeyStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                //该属性有值，先缓存属性名称，后续交给其他状态处理
                case '=':
                    machine.propKey = machine.MergeChar ();
                    machine.EnterStats (machine.tagPropValStatus);
                    break;

                //该属性名称没有值，直接缓存
                case ' ':
                case '/':
                case '>':
                    machine.propKey = machine.MergeChar ();
                    machine.propVal = "";
                    machine.SavePropKV ();
                    break;

                default:
                    machine.AddChar (content);
                    break;
            }

            switch ( content )
            {
                case ' ':
                    machine.EnterStats (machine.tagPropSpaceStatus);
                    break;

                case '/':
                case '>':
                    machine.EnterStats (machine.tagPropEndStatus);
                    break;
            }
        }
    }
}
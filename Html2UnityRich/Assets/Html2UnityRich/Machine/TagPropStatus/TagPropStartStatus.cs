namespace Html2UnityRich
{
    public class TagPropStartStatus : TagPropStatus
    {
        public TagPropStartStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                case ' ':
                case '/':
                case '>':
                    machine.tagName = machine.MergeChar ();
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
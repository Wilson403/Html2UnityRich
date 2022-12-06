namespace Html4UnityText
{
    public class TagPropValStartStatus : TagPropStatus
    {
        public TagPropValStartStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                case '"':
                    machine.propVal = machine.MergeChar ();
                    machine.SavePropKV ();
                    machine.EnterStats (machine.tagPropValEndStatus);
                    break;

                default:
                    machine.AddChar (content);
                    break;
            }
        }
    }
}
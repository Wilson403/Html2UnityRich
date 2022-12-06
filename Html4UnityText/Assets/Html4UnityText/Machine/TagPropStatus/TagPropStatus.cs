namespace Html4UnityText
{
    public abstract class TagPropStatus
    {
        /// <summary>
        /// 进入状态
        /// </summary>
        public virtual void Enter ()
        {

        }

        /// <summary>
        /// 离开状态
        /// </summary>
        public virtual void Exit ()
        {

        }

        /// <summary>
        /// 添加一个字符
        /// </summary>
        /// <param name="content"></param>
        public virtual void ApendChar (char content)
        {

        }

        /// <summary>
        /// 停止字符的输入
        /// </summary>
        public virtual void EndApend ()
        {

        }

        /// <summary>
        /// 状态机对象
        /// </summary>
        protected TagPropMachine machine;

        public TagPropStatus (TagPropMachine machine)
        {
            this.machine = machine;
        }
    }
}
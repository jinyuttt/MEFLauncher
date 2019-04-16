namespace MEFLib
{
    public delegate void MainViewExit(object sender, object info=null);
   public interface IMainView
    {
        /// <summary>
        /// 标识
        /// </summary>
        string ViewName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        string DisplayName { get; set; }

        event MainViewExit ViewExitEvent;
        void Show();
        void Close();
    }
}

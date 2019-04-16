namespace MEFLib
{
    public  interface IView
    {
        /// <summary>
        /// 标识
        /// </summary>
        string ViewName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        string DisplayName { get; set; }

        object GetView(string name);
        void Show();
        void Close();
    }
}

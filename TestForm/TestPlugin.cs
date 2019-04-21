using MEFLib;

namespace TestForm
{
    public class TestPlugin : IView
    {
        private string name = "TestForm";
        public string ViewName { get { return name; }  set { name = value; } }
        public string DisplayName { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Close()
        {
            
        }

        public object GetView(string name)
        {
            return new  FrmMain();
        }

        public void Show()
        {
           
        }
    }
}

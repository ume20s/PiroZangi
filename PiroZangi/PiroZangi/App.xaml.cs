using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiroZangi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        protected override void OnResume()
        {
        }
    }
}

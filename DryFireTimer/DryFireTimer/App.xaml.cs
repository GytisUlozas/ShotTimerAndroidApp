using DryFireTimer.DataAccess;
using DryFireTimer.Pages;
using System;
using System.IO;
using Xamarin.Forms;

namespace DryFireTimer
{
    public partial class App : Application
    {
        private readonly MainPage firstPage;
        private readonly string dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "database.db3");
        private static IMyRepository repo;
        public static string AudioName => "shot_timer.wav";
        
        public App()
        {
            InitializeComponent();
            repo = new Repository(dbpath);
            firstPage = new MainPage(repo);
            MainPage = firstPage;
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
          
        }

        protected override void OnResume()
        {
            
        }
    }
}

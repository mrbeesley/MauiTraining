namespace MyFirstApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            //MainPage = new FirstPage();
            //MainPage = new LoginPage();
            //MainPage = new ScorllViewPage();
            //MainPage = new FlexLayoutPage();
            MainPage = new GridPage();
        }
    }
}
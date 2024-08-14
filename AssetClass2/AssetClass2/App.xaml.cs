using System.Configuration;
using System.Data;
using System.Windows;

namespace AssetClass2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string ConnectionString { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConnectionString = "Server = localhost; Database = assetclassanalyzer; uid = root; Pwd = password;";
        }

    }

}

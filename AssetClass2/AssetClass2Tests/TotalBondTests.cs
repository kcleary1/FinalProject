using Xunit;
using System;
using System.Windows;
using MySql.Data.MySqlClient;
using AssetClass2;
using System.Windows.Threading;

namespace AssetClass2Tests
{
    public class TotalBondTests : IDisposable
    {
        private readonly MainWindow _window;
        private readonly string _connectionString;

        public TotalBondTests()
        {
            _connectionString = "Server=localhost;Database=assetclassanalyzer;Uid=root;Pwd=password;";

            Exception initException = null;
            _window = new MainWindow();

            _window.Dispatcher.Invoke(() =>
            {
                try
                {
                    _window.InitializeComponent();
                    _window.Show();
                }
                catch (Exception ex)
                {
                    initException = ex;
                    Console.WriteLine($"Exception during window initialization: {ex}");
                }
            });

            if (initException != null)
            {
                throw new Exception("Window initialization failed", initException);
            }

            // Wait for UI to finish loading
            System.Threading.Thread.Sleep(1000);
        }

        [WpfTheory]
        [InlineData("US Total Bond", 1, 867.50, 1024.70, 1181.80)]
        [InlineData("US Total Bond", 5, 932.88, 1261.76, 1676.73)]
        [InlineData("US Total Bond", 10, 1162.83, 1630.45, 2260.98)]
        [InlineData("US Total Bond", 15, 1461.07, 2105.82, 3008.81)]
        [InlineData("US Total Bond", 20, 1845.08, 2714.61, 3964.81)]
        public void TotalBondTesting(string index, int years, double expectedLowest, double expectedMean, double expectedHighest)
        {
            Exception testException = null;
            _window.Dispatcher.Invoke(() =>
            {
                try
                {
                    // Arrange
                    if (_window.IndexDropDownControl.Items.Contains(index))
                        _window.IndexDropDownControl.SelectedItem = index;
                    else
                        throw new Exception($"Index '{index}' not found in IndexDropDownControl");

                    if (_window.YearDropDownControl.Items.Contains(years))
                        _window.YearDropDownControl.SelectedItem = years;
                    else
                        throw new Exception($"Year '{years}' not found in YearDropDownControl");

                    // Act
                    _window.UpdateResults();

                    // Assert
                    Assert.Equal(expectedLowest.ToString("C2"), _window.Lowest1000TextBox.Text);
                    Assert.Equal(expectedMean.ToString("C2"), _window.Mean1000TextBox.Text);
                    Assert.Equal(expectedHighest.ToString("C2"), _window.Highest1000TextBox.Text);
                }
                catch (Exception ex)
                {
                    testException = ex;
                    Console.WriteLine($"Exception during test: {ex}");
                }
            });

            if (testException != null)
            {
                throw testException;
            }
        }

        public void Dispose()
        {
            _window.Dispatcher.Invoke(() =>
            {
                _window.Close();
            });
        }
    }
}
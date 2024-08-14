using Xunit;
using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data;
using MySql.Data.MySqlClient;
using AssetClass2;

namespace AssetClass2Tests
{
    public class LargeGrowthTests
    {

        public readonly MainWindow _window;
        private readonly string _connectionString;


        public LargeGrowthTests()
        {
            _connectionString = "Server = localhost; Database = assetclassanalyzer; uid = root; Pwd = password;";
            _window = new MainWindow();
            _window._assetClassRepository = new AssetClassRepository(_connectionString);
        }

        [WpfTheory]
        [InlineData("US Large Growth", 1, 616.80, 1248.50, 1880.20)]
        [InlineData("US Large Growth", 5, 726.52, 2039.50, 4796.30)]
        [InlineData("US Large Growth", 10, 1343.92, 3626.72, 8940.15)]
        [InlineData("US Large Growth", 15, 2420.41, 6525.27, 16521.47)]
        [InlineData("US Large Growth", 20, 4550.02, 11937.92, 29911.13)]
        public void LargeGrowthTesting(string index, int years, double expectedLowest, double expectedMean, double expectedHighest)
        {


            //Arrange
            _window.IndexDropDownControl.SelectedItem = index;
            _window.YearDropDownControl.SelectedItem = years;

            //Act
            _window.UpdateResults();

            //Assert

            Assert.Equal(expectedLowest.ToString("C2"), _window.Lowest1000TextBox.Text);
            Assert.Equal(expectedMean.ToString("C2"), _window.Mean1000TextBox.Text);
            Assert.Equal(expectedHighest.ToString("C2"), _window.Highest1000TextBox.Text);

        }
    }
}
   
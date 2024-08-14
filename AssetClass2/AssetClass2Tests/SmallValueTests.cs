using Xunit;
using System;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;
using AssetClass2;


namespace AssetClass2Tests
{
    public class SmallValueTests
    {

        private readonly MainWindow _window;
        private readonly string _connectionString;


        public SmallValueTests()
        {
            _connectionString = "Server = localhost; Database = assetclassanalyzer; uid = root; Pwd = password;";
            _window = new MainWindow();
            _window._assetClassRepository = new AssetClassRepository(_connectionString);
        }

        [WpfTheory]
        [InlineData("US Small Value", 1, 679.50, 1232.90, 1786.20)]
        [InlineData("US Small Value", 5, 817.07, 1978.34, 4192.96)]
        [InlineData("US Small Value", 10, 1273.85, 3519.76, 8854.34)]
        [InlineData("US Small Value", 15, 2087.86, 6114.61, 16645.23)]
        [InlineData("US Small Value", 20, 3597.15, 10791.50, 30522.69)]
        public void SmallValueTesting(string index, int years, double expectedLowest, double expectedMean, double expectedHighest)
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


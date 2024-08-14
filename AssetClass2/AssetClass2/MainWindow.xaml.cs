using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace AssetClass2
{
    public partial class MainWindow : Window
    {
        public AssetClassRepository _assetClassRepository;
        private string _connectionString;

        
        public ComboBox IndexDropDownControl => IndexDropDown;
        public ComboBox YearDropDownControl => YearDropDown;

        public TextBox Lowest1000TextBox => Lowest1000;
        public TextBox Mean1000TextBox => Mean1000;
        public TextBox Highest1000TextBox => Highest1000;



        public MainWindow()
        {
            InitializeComponent();
            _connectionString = App.ConnectionString;
            _assetClassRepository = new AssetClassRepository(_connectionString);

            LoadAssetClasses();
            PopulateDropdowns();

            IndexDropDown.SelectionChanged += DropDown_SelectionChanged;
            YearDropDown.SelectionChanged += DropDown_SelectionChanged;
            
        }

        public void LoadAssetClasses()
        {
            var largeGrowth = _assetClassRepository.GetLargeGrowthInfo();
            var smallValue = _assetClassRepository.GetSmallValueInfo();
            var totalBond = _assetClassRepository.GetTotalBondInfo();
        }

        public void PopulateDropdowns()
        {
            IndexDropDown.ItemsSource = new List<string> { "US Large Growth", "US Small Value", "US Total Bond" };
            YearDropDown.ItemsSource = Enumerable.Range(1, 20).ToList();
        }

        public void DropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateResults();
        }

        

        public void UpdateResults()
        {
            string selectedIndex = IndexDropDown.SelectedItem as string;
            int? selectedYear = YearDropDown.SelectedItem as int?;

            if (string.IsNullOrEmpty(selectedIndex) || !selectedYear.HasValue)
            {
                return;
            }

            string tableName = GetTableName(selectedIndex);
            if (string.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Please select a valid index.");
                return;
            }

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = $"SELECT Lowest, Average, Highest FROM {tableName} WHERE Years = @years";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Years", selectedYear.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                double lowest = reader.GetDouble(0);
                                double average = reader.GetDouble(1);
                                double highest = reader.GetDouble(2);

                                Lowest1000.Text = (1000 * Math.Pow(1 + lowest, selectedYear.Value)).ToString("C2");
                                Mean1000.Text = (1000 * Math.Pow(1 + average, selectedYear.Value)).ToString("C2");
                                Highest1000.Text = (1000 * Math.Pow(1 + highest, selectedYear.Value)).ToString("C2");
                            }
                            else
                            {
                                MessageBox.Show("No data found for the selected year and index.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public string GetTableName(string index)
        {
            switch (index)
            {
                case "US Large Growth":
                    return "uslargegrowth";
                case "US Small Value":
                    return "ussmallvalue";
                case "US Total Bond":
                    return "ustotalbond";
                default:
                    return string.Empty;
            }
        }
    }
}
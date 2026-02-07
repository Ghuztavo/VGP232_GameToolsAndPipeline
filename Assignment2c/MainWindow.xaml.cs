using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WeaponLib;
using WeaponType = WeaponLib.Weapon.WeaponType;

namespace Assignment2c
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private WeaponCollection mWeaponCollection;
        private List<Weapon> mAllWeapons;
        public MainWindow()
        {
            InitializeComponent();

            mWeaponCollection = new WeaponCollection();
            mAllWeapons = new List<Weapon>();
            WeaponsListBox.ItemsSource = mWeaponCollection;

            ShowTypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponType));

            ShowTypeComboBox.SelectedItem = WeaponType.None;
            ShowTypeComboBox.SelectionChanged += ShowTypeComboBox_SelectionChanged;
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            string path = "data2.csv";

            if (mWeaponCollection.Load(path))
            {
                // Keep a copy of all weapons for filtering
                mAllWeapons = new List<Weapon>(mWeaponCollection);

                ApplyFilter();
            }
            else
            {
                MessageBox.Show("Failed to load weapons.");
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string path = "output.csv";

            if (!mWeaponCollection.Save(path))
            {
                MessageBox.Show("Failed to save weapons.");
            }
        }

        private void SortRadio_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio && radio.IsChecked == true)
            {
                string sortColumn = radio.Content.ToString();

                try
                {
                    mWeaponCollection.SortBy(sortColumn);
                    WeaponsListBox.Items.Refresh();
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void ShowTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (mAllWeapons.Count == 0)
                return;

            mWeaponCollection.Clear();

            WeaponType selectedType = (WeaponType)ShowTypeComboBox.SelectedItem;
            string filterName = FilterByNameBox.Text;

            foreach (var weapon in mAllWeapons)
            {
                bool typeMatch = (selectedType == WeaponType.None || weapon.Type == selectedType);
                bool nameMatch = string.IsNullOrEmpty(filterName) || 
                                 (weapon.Name != null && weapon.Name.Contains(filterName, StringComparison.OrdinalIgnoreCase));

                if (typeMatch && nameMatch)
                {
                    mWeaponCollection.Add(weapon);
                }
            }

            WeaponsListBox.Items.Refresh();
        }

        private void RemoveWeapon_Click(object sender, RoutedEventArgs e)
        {
            if (WeaponsListBox.SelectedItem is not Weapon selectedWeapon)
            {
                MessageBox.Show("Please select a weapon to remove.");
                return;
            }

            // Remove from the full list
            mAllWeapons.Remove(selectedWeapon);

            // Remove from the currently displayed list
            mWeaponCollection.Remove(selectedWeapon);

            WeaponsListBox.Items.Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWeapon addWeaponWindow = new AddWeapon();
            if (addWeaponWindow.ShowDialog() == true)
            {
                Weapon newWeapon = addWeaponWindow.NewWeapon;
                if (newWeapon != null)
                {
                    mAllWeapons.Add(newWeapon);
                    mWeaponCollection.Add(newWeapon);
                    
                    WeaponsListBox.Items.Refresh();
                }
            }
        }

        private void EditWeaponButton_Click(object sender, RoutedEventArgs e)
        {
            if (WeaponsListBox.SelectedItem is Weapon selectedWeapon)
            {
                EditWeapon editWeaponWindow = new EditWeapon();
                editWeaponWindow.Initialize(selectedWeapon);
                
                if (editWeaponWindow.ShowDialog() == true)
                {
                   WeaponsListBox.Items.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a weapon to edit.");
            }
        }

        private void FilterByNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilter();
        }
    }
}
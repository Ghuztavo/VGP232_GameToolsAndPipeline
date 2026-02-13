using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using WeaponLib;

namespace Assignment2c
{
    /// <summary>
    /// Interaction logic for AddWeapon.xaml
    /// </summary>
    public partial class AddWeapon : Window
    {
        public Weapon NewWeapon { get; private set; }

        public AddWeapon()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            AddWeaponTypeComboBox.ItemsSource = Enum.GetValues(typeof(Weapon.WeaponType));
            AddWeaponRariyComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };
        }

        private void AutoGenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            // Random BaseAttack between 20 and 50
            int baseAttack = random.Next(20, 51);
            AddWeaponBaseAttackBox.Text = baseAttack.ToString();

            // Random Type
            Array values = Enum.GetValues(typeof(Weapon.WeaponType));
            Weapon.WeaponType randomType = (Weapon.WeaponType)values.GetValue(random.Next(values.Length));
            AddWeaponTypeComboBox.SelectedItem = randomType;

            // Random Rarity (1-5)
            int rarity = random.Next(1, 6);
            AddWeaponRariyComboBox.SelectedItem = rarity;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Checks if required fields are filled and valid
            if (string.IsNullOrWhiteSpace(AddWeaponNameBox.Text) ||
                AddWeaponTypeComboBox.SelectedItem == null ||
                AddWeaponRariyComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(AddWeaponBaseAttackBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!int.TryParse(AddWeaponBaseAttackBox.Text, out int baseAttack))
            {
                MessageBox.Show("Base Attack must be a valid number.");
                return;
            }

            // Create new weapon based on user input
            int rarity = (int)AddWeaponRariyComboBox.SelectedItem;
            Weapon.WeaponType type = (Weapon.WeaponType)AddWeaponTypeComboBox.SelectedItem;

            NewWeapon = new Weapon
            {
                Name = AddWeaponNameBox.Text,
                Type = type,
                Rarity = rarity,
                BaseAttack = baseAttack,
                Image = AddWeaponImageURLBox.Text,
                SecondaryStat = AddWeaponSecondaryStatBox.Text,
                Passive = AddWeaponPassiveBox.Text
            };

            DialogResult = true;
            Close();
        }

        // Helper to update image when text changes
        private void ImageUrlBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(AddWeaponImageURLBox.Text))
                {
                    AddWeaponImage.Source = new BitmapImage(new Uri(AddWeaponImageURLBox.Text));
                }
                else
                {
                    AddWeaponImage.Source = null;
                }
            }
            catch
            {
                // Invalid URL
                AddWeaponImage.Source = null;
            }
        }
    }
}

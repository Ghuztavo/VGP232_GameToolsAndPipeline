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

namespace Assignment2c
{
    /// <summary>
    /// Interaction logic for EditWeapon.xaml
    /// </summary>
    public partial class EditWeapon : Window
    {
        public WeaponLib.Weapon WeaponToEdit { get; set; }

        public EditWeapon()
        {
            InitializeComponent();
            InitializeComboBoxes();
        }

        private void InitializeComboBoxes()
        {
            EditWeaponTypeComboBox.ItemsSource = Enum.GetValues(typeof(WeaponLib.Weapon.WeaponType));
            EditWeaponRarityComboBox.ItemsSource = new List<int> { 1, 2, 3, 4, 5 };
        }

        public void Initialize(WeaponLib.Weapon weapon)
        {
            WeaponToEdit = weapon;
            if (WeaponToEdit != null)
            {
                EditWeaponNameBox.Text = WeaponToEdit.Name;
                EditWeaponTypeComboBox.SelectedItem = WeaponToEdit.Type;
                EditWeaponRarityComboBox.SelectedItem = WeaponToEdit.Rarity;
                EditWeaponBaseAttackBox.Text = WeaponToEdit.BaseAttack.ToString();
                EditWeaponImageURLBox.Text = WeaponToEdit.Image;
                EditWeaponSecondaryStatBox.Text = WeaponToEdit.SecondaryStat;
                EditWeaponPassiveBox.Text = WeaponToEdit.Passive;
                
                UpdateImage();
            }
        }

        private void EditWeaponSaveButton_Click(object sender, RoutedEventArgs e)
        {
             if (string.IsNullOrWhiteSpace(EditWeaponNameBox.Text) ||
                EditWeaponTypeComboBox.SelectedItem == null ||
                EditWeaponRarityComboBox.SelectedItem == null ||
                string.IsNullOrWhiteSpace(EditWeaponBaseAttackBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            if (!int.TryParse(EditWeaponBaseAttackBox.Text, out int baseAttack))
            {
                MessageBox.Show("Base Attack must be a valid number.");
                return;
            }

            // Update the weapon
            WeaponToEdit.Name = EditWeaponNameBox.Text;
            WeaponToEdit.Type = (WeaponLib.Weapon.WeaponType)EditWeaponTypeComboBox.SelectedItem;
            WeaponToEdit.Rarity = (int)EditWeaponRarityComboBox.SelectedItem;
            WeaponToEdit.BaseAttack = baseAttack;
            WeaponToEdit.Image = EditWeaponImageURLBox.Text;
            WeaponToEdit.SecondaryStat = EditWeaponSecondaryStatBox.Text;
            WeaponToEdit.Passive = EditWeaponPassiveBox.Text;

            DialogResult = true;
            Close();
        }

        private void EditWeaponCancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void EditWeaponAutoGenerateButton_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();

            int baseAttack = random.Next(20, 51);
            EditWeaponBaseAttackBox.Text = baseAttack.ToString();

            Array values = Enum.GetValues(typeof(WeaponLib.Weapon.WeaponType));
            WeaponLib.Weapon.WeaponType randomType = (WeaponLib.Weapon.WeaponType)values.GetValue(random.Next(values.Length));
            EditWeaponTypeComboBox.SelectedItem = randomType;

            int rarity = random.Next(1, 6);
            EditWeaponRarityComboBox.SelectedItem = rarity;
        }

        private void EditWeaponImageURLBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateImage();
        }

        private void UpdateImage()
        {
             try
            {
                if (!string.IsNullOrWhiteSpace(EditWeaponImageURLBox.Text))
                {
                    EditWeaponImage.Source = new BitmapImage(new Uri(EditWeaponImageURLBox.Text));
                }
                else
                {
                    EditWeaponImage.Source = null;
                }
            }
            catch
            {
                EditWeaponImage.Source = null;
            }
        }
    }
}

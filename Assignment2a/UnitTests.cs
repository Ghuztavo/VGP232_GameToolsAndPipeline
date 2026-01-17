using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using WeaponType = Assignment2a.Weapon.WeaponType;

namespace Assignment2a
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection weaponCollection;
        private string inputPath;
        private string outputPath;

        const string INPUT_FILE = "data2.csv";
        const string OUTPUT_FILE = "output.csv";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            outputPath = CombineToAppPath(OUTPUT_FILE);
            weaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(outputPath))
            {
                // coment if I want to keep the output file to check
                File.Delete(outputPath);
            }
        }

        // WeaponCollection Unit Tests
        [Test]
        public void WeaponCollection_GetHighestBaseAttack_HighestValue()
        {
            // Expected Value: 48
            // TODO: call WeaponCollection.GetHighestBaseAttack() and confirm that it matches the expected value using asserts.
            Assert.That(weaponCollection.Load(inputPath));

            int highest = weaponCollection.GetHighestBaseAttack();

            Assert.That(highest, Is.EqualTo(48));
        }

        [Test]
        public void WeaponCollection_GetLowestBaseAttack_LowestValue()
        {
            // Expected Value: 23
            // TODO: call WeaponCollection.GetLowestBaseAttack() and confirm that it matches the expected value using asserts.
            Assert.That(weaponCollection.Load(inputPath));

            int lowest = weaponCollection.GetLowestBaseAttack();

            Assert.That(lowest, Is.EqualTo(23));
        }

        [TestCase(WeaponType.Sword, 21)]
        public void WeaponCollection_GetAllWeaponsOfType_ListOfWeapons(WeaponType type, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfType(type) and confirm that the weapons list returns Count matches the expected value using asserts.
            Assert.That(weaponCollection.Load(inputPath));

            var result = weaponCollection.GetAllWeaponsOfType(type);

            Assert.That(result.Count, Is.EqualTo(expectedValue));
        }

        [TestCase(5, 10)]
        public void WeaponCollection_GetAllWeaponsOfRarity_ListOfWeapons(int stars, int expectedValue)
        {
            // TODO: call WeaponCollection.GetAllWeaponsOfRarity(stars) and confirm that the weapons list returns Count matches the expected value using asserts.
            Assert.That(weaponCollection.Load(inputPath));

            var result = weaponCollection.GetAllWeaponsOfRarity(stars);

            Assert.That(result.Count, Is.EqualTo(expectedValue));
        }

        [Test]
        public void WeaponCollection_LoadThatExistAndValid_True()
        {
            // TODO: load returns true, expect WeaponCollection with count of 95 .
            Assert.That(weaponCollection.Load(inputPath));

            bool loaded = weaponCollection.Load(inputPath);

            Assert.That(loaded, Is.True);
        }

        [Test]
        public void WeaponCollection_LoadThatDoesNotExist_FalseAndEmpty()
        {
            // TODO: load returns false, expect an empty WeaponCollection
            bool loaded = weaponCollection.Load("does_not_exist.csv");

            Assert.That(loaded, Is.False);

            Assert.That(weaponCollection.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_SaveWithValuesCanLoad_TrueAndNotEmpty()
        {
            // TODO: save returns true, load returns true, and WeaponCollection is not empty.
            Assert.That(weaponCollection.Load(inputPath));
            Assert.That(weaponCollection.Save(outputPath));
            Assert.That(weaponCollection.Load(outputPath));
            Assert.That(weaponCollection.Count, Is.GreaterThan(0));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_TrueAndEmpty()
        {
            // After saving an empty WeaponCollection, load the file and expect WeaponCollection to be empty.
            weaponCollection.Clear();
            Assert.That(weaponCollection.Save(outputPath));
            Assert.That(weaponCollection.Load(outputPath));
            Assert.That(weaponCollection.Count, Is.EqualTo(0));
        }

        // Weapon Unit Tests
        [Test]
        public void Weapon_TryParseValidLine_TruePropertiesSet()
        {
            // TODO: create a Weapon with the stats above set properly
            Weapon expected = null;
            // TODO: uncomment this once you added the Type1 and Type2
            expected = new Weapon()
            {
                Name = "Skyward Blade",
                Type = Weapon.WeaponType.Sword,
                Image = "https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png",
                Rarity = 5,
                BaseAttack = 46,
                SecondaryStat = "Energy Recharge",
                Passive = "Sky-Piercing Fang"
            };

            string line = "Skyward Blade,Sword,https://vignette.wikia.nocookie.net/gensin-impact/images/0/03/Weapon_Skyward_Blade.png,5,46,Energy Recharge,Sky-Piercing Fang";
            Weapon actual = null;

            // TODO: uncomment this once you have TryParse implemented.
            Assert.That(Weapon.TryParse(line, out actual), Is.True);

            Assert.That(expected.Name, Is.EqualTo(actual.Name));
            Assert.That(expected.Type, Is.EqualTo(actual.Type));
            Assert.That(expected.Image, Is.EqualTo(actual.Image));
            Assert.That(expected.Rarity, Is.EqualTo(actual.Rarity));
            Assert.That(expected.BaseAttack, Is.EqualTo(actual.BaseAttack));
            Assert.That(expected.SecondaryStat, Is.EqualTo(actual.SecondaryStat));
            Assert.That(expected.Passive, Is.EqualTo(actual.Passive));
            // TODO: check for the rest of the properties, Image,Rarity,SecondaryStat,Passive

        }

        [Test]
        public void Weapon_TryParseInvalidLine_FalseNull()
        {
            // TODO: use "1,Bulbasaur,A,B,C,65,65", Weapon.TryParse returns false, and Weapon is null.
            string line = "1,Bulbasaur,A,B,C,65,65";

            bool result = Weapon.TryParse(line, out Weapon weapon);

            Assert.That(result, Is.False);
            Assert.That(weapon, Is.Null);
        }
    }
}

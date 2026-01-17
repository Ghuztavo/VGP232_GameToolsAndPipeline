using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WeaponType = Assignment2a.Weapon.WeaponType;

namespace Assignment2a
{
    class WeaponCollection : List<Weapon>, IPeristence
    {
        public bool Load(string filename)
        {
            if (new FileInfo(filename).Length == 0)
                return true;

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    reader.ReadLine(); // Skip header

                    // Read each line
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (Weapon.TryParse(line, out Weapon weapon))
                        {
                            this.Add(weapon);
                        }
                        // invalid rows are ignored but handled inside TryParse
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading file: {ex.Message}");
                return false;
            }
        }

        public bool Save(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    // Write header
                    writer.WriteLine("Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive");

                    // Write each weapon
                    foreach (var weapon in this)
                    {
                        writer.WriteLine(weapon.ToString());
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
                return false;
            }
        }

        // get the highest base attack
        public int GetHighestBaseAttack()
        {
            int highest = 0;
            foreach (var weapon in this)
            {
                if (weapon.BaseAttack > highest)
                {
                    highest = weapon.BaseAttack;
                }
            }
            return highest;
        }


        // get lowest base attack
        public int GetLowestBaseAttack()
        {
            int lowest = int.MaxValue;
            foreach (var weapon in this)
            {
                if (weapon.BaseAttack < lowest)
                {
                    lowest = weapon.BaseAttack;
                }
            }
            return lowest;
        }

        // get all weapons of a specific type
        public List<Weapon> GetAllWeaponsOfType(WeaponType type)
        {
            List<Weapon> weaponsOfType = new List<Weapon>();
            foreach (var weapon in this)
            {
                if (weapon.Type == type)
                {
                    weaponsOfType.Add(weapon);
                }
            }
            return weaponsOfType;
        }

        // get all weapons of a specific rarity
        public List<Weapon> GetAllWeaponsOfRarity(int rarity)
        {
            List<Weapon> weaponsOfRarity = new List<Weapon>();
            foreach (var weapon in this)
            {
                if (weapon.Rarity == rarity)
                {
                    weaponsOfRarity.Add(weapon);
                }
            }
            return weaponsOfRarity;
        }

        public void SortBy(string columnName)
        {
            Comparison<Weapon> comparison = columnName.ToLower() switch
            {
                "name" => Weapon.CompareByName,
                "type" => Weapon.CompareByType,
                "rarity" => Weapon.CompareByRarity,
                "baseattack" => Weapon.CompareByBaseAttack,
                _ => throw new ArgumentException($"Invalid column name: {columnName}")
            };
            this.Sort(comparison);
        }

    }
}


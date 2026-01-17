using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment2a
{
    public class Weapon
    {
        public enum WeaponType
        {
            Sword, Polearm, Claymore, Catalyst, Bow, None
        }

        // Name,Type,Rarity,BaseAttack
        public string Name { get; set; }
        public WeaponType Type { get; set; }
        public int Rarity { get; set; }
        public int BaseAttack { get; set; }
        public string Image { get; set; }
        public string SecondaryStat { get; set; }
        public string Passive { get; set; }

        /// <summary>
        /// The Comparator function to check for name
        /// </summary>
        /// <param name="left">Left side Weapon</param>
        /// <param name="right">Right side Weapon</param>
        /// <returns> -1 (or any other negative value) for "less than", 0 for "equals", or 1 (or any other positive value) for "greater than"</returns>
        public static int CompareByName(Weapon left, Weapon right)
        {
            return left.Name.CompareTo(right.Name);
        }

        public static int CompareByType(Weapon left, Weapon right)
        {
            return left.Type.CompareTo(right.Type);
        }

        public static int CompareByRarity(Weapon left, Weapon right)
        {
            return left.Rarity.CompareTo(right.Rarity);
        }

        public static int CompareByBaseAttack(Weapon left, Weapon right)
        {
            return left.BaseAttack.CompareTo(right.BaseAttack);
        }

        /// <summary>
        /// The Weapon string with all the properties
        /// </summary>
        /// <returns>The Weapon formated string</returns>
        public override string ToString()
        {
            // TODO: construct a comma seperated value string
            // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive

            return $"{Name},{Type},{Image},{Rarity},{BaseAttack},{SecondaryStat},{Passive}";
        }

        public static bool TryParse(string rawData, out Weapon weapon)
        {
            // TODO: validate that the string array the size expected.
            // TODO: use int.Parse or TryParse for stats/number values.
            // Populate the properties of the Weapon
            // TODO: Add the Weapon to the list

            weapon = null;

            if (string.IsNullOrWhiteSpace(rawData))
            {
                Console.WriteLine("Input data is null or empty.");
                return false;
            }

            // Order:
            // Name,Type,Image,Rarity,BaseAttack,SecondaryStat,Passive
            string[] values = rawData.Split(',');

            if (values.Length != 7)
            {
                Console.WriteLine($"Input data does not have the correct number of fields: {rawData}");
                return false;
            }

            // Parse WeaponType
            if (!Enum.TryParse(values[1], true, out WeaponType type))
            {
                Console.WriteLine($"Invalid WeaponType value: {values[1]}");
                type = WeaponType.None;
            }

            // Parse Rarity
            if (!int.TryParse(values[3], out int rarity))
            {
                Console.WriteLine($"Invalid Rarity value: {values[3]}");
                return false;
            }

            // Parse BaseAttack
            if (!int.TryParse(values[4], out int baseAttack))
            {
                Console.WriteLine($"Invalid BaseAttack value: {values[4]}");
                return false;
            }

            weapon = new Weapon
            {
                Name = values[0],
                Type = type,
                Image = values[2],
                Rarity = rarity,
                BaseAttack = baseAttack,
                SecondaryStat = values[5],
                Passive = values[6]
            };

            return true;
        }
    }
}

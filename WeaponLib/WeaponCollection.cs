using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WeaponType = WeaponLib.Weapon.WeaponType;

namespace WeaponLib
{
    public class WeaponCollection : List<Weapon>, IPersistence, IXmlSerializable, IJsonSerializable, ICsvSerializable
    {
        public bool Load(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename).ToLower();

            return extension switch
            {
                ".xml" => LoadXML(filename),
                ".json" => LoadJSON(filename),
                ".csv" => LoadCSV(filename),
                _ => throw new NotSupportedException(
                        $"Load: Unsupported file extension '{extension}'")
            };
        }

        public bool Save(string filename)
        {
            string extension = System.IO.Path.GetExtension(filename).ToLower();

            return extension switch
            {
                ".xml" => SaveXML(filename),
                ".json" => SaveJSON(filename),
                ".csv" => SaveCSV(filename),
                _ => throw new NotSupportedException(
                        $"Save: Unsupported file extension '{extension}'")
            };
        }

        // XML
        public bool LoadXML(string filename)
        {
            if (!File.Exists(filename))
                return false;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));

                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    List<Weapon> weapons = (List<Weapon>)serializer.Deserialize(fs);

                    this.Clear();
                    this.AddRange(weapons);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML load error: {ex.Message}");
                return false;
            }
        }
        public bool SaveXML(string filename)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Weapon>));

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    serializer.Serialize(fs, this.ToList());
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"XML save error: {ex.Message}");
                return false;
            }
        }

        // JSON
        public bool LoadJSON(string filename)
        {
            if (!File.Exists(filename))
                return false;

            try
            {
                string json = File.ReadAllText(filename);

                List<Weapon>? weapons = JsonSerializer.Deserialize<List<Weapon>>(json);

                if (weapons == null)
                    return false;

                this.Clear();
                this.AddRange(weapons);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON load error: {ex.Message}");
                return false;
            }
        }
        public bool SaveJSON(string filename)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(this.ToList(), options);
                File.WriteAllText(filename, json);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"JSON save error: {ex.Message}");
                return false;
            }
        }

        // CSV
        public bool LoadCSV(string filename)
        {
            if (!File.Exists(filename))
                return false;

            try
            {
                this.Clear();

                using (StreamReader reader = new StreamReader(filename))
                {
                    reader.ReadLine(); // Skip header

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();

                        if (Weapon.TryParse(line, out Weapon weapon))
                        {
                            this.Add(weapon);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV: {ex.Message}");
                return false;
            }
        }

        public bool SaveCSV(string filename)
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
                "passive" => Weapon.CompareByPassive,
                "secondarystat" => Weapon.CompareBySecondaryStat,
                _ => throw new ArgumentException($"Invalid column name: {columnName}")
            };
            this.Sort(comparison);
        }

    }
}


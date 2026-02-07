using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeaponType = WeaponLib.Weapon.WeaponType;

namespace WeaponLib
{
    [TestFixture]
    public class UnitTests
    {
        private WeaponCollection weaponCollection;
        private string inputPath;
        private string csvOutputPath;
        private string jsonOutputPath;
        private string xmlOutputPath;

        const string INPUT_FILE = "data2.csv";
        const string CSV_OUTPUT_FILE = "output.csv";
        const string OUTPUT_XML_FILE = "output.xml";
        const string OUTPUT_JSON_FILE = "output.json";

        // A helper function to get the directory of where the actual path is.
        private string CombineToAppPath(string filename)
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
        }

        [SetUp]
        public void SetUp()
        {
            inputPath = CombineToAppPath(INPUT_FILE);
            csvOutputPath = CombineToAppPath(CSV_OUTPUT_FILE);
            jsonOutputPath = CombineToAppPath(OUTPUT_JSON_FILE);
            xmlOutputPath = CombineToAppPath(OUTPUT_XML_FILE);
            weaponCollection = new WeaponCollection();
        }

        [TearDown]
        public void CleanUp()
        {
            // We remove the output file after we are done.
            if (File.Exists(csvOutputPath)) File.Delete(csvOutputPath); // coment if you want to keep the csv file
            if (File.Exists(jsonOutputPath)) File.Delete(jsonOutputPath); // coment if you want to keep the json file
            if (File.Exists(xmlOutputPath)) File.Delete(xmlOutputPath); // coment if you want to keep the xml file
        }

        // JSON Persistence Unit Tests -------------------------------------------------------------
        [Test]
        public void WeaponCollection_Load_Save_Load_ValidJson()
        {
            // Load CSV
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Save checking the extension
            Assert.That(weaponCollection.Save(jsonOutputPath), Is.True);

            // Load checking the extension
            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(jsonOutputPath), Is.True);
            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_Load_ValidJson()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            Assert.That(weaponCollection.SaveJSON(jsonOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(jsonOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsJSON_LoadJSON_ValidJson()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            Assert.That(weaponCollection.SaveJSON(jsonOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.LoadJSON(jsonOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_Save_LoadJSON_ValidJson()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Save() chooses JSON by extension
            Assert.That(weaponCollection.Save(jsonOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.LoadJSON(jsonOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidJson()
        {
            // Ensure collection is empty
            weaponCollection.Clear();
            Assert.That(weaponCollection.Count, Is.EqualTo(0));

            // Save empty collection as JSON
            Assert.That(weaponCollection.SaveJSON(jsonOutputPath), Is.True);

            // Load using extension-based Load()
            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(jsonOutputPath), Is.True);
            // Verify still empty
            Assert.That(output.Count, Is.EqualTo(0));
        }

        // XML Persistence Unit Tests -------------------------------------------------------------

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidXml()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Save using extension-based Save()
            Assert.That(weaponCollection.Save(xmlOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(xmlOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsXML_LoadXML_ValidXml()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Explicit XML save
            Assert.That(weaponCollection.SaveXML(xmlOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.LoadXML(xmlOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidXml()
        {
            weaponCollection.Clear();
            Assert.That(weaponCollection.Count, Is.EqualTo(0));

            Assert.That(weaponCollection.SaveXML(xmlOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(xmlOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(0));
        }

        // CSV Persistence Unit Tests -------------------------------------------------------------

        [Test]
        public void WeaponCollection_Load_Save_Load_ValidCsv()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Save checking the extension
            Assert.That(weaponCollection.Save(csvOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(csvOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_Load_SaveAsCSV_LoadCSV_ValidCsv()
        {
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Explicit CSV save
            Assert.That(weaponCollection.SaveCSV(csvOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.LoadCSV(csvOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(95));
        }

        [Test]
        public void WeaponCollection_SaveEmpty_Load_ValidCsv()
        {
            weaponCollection.Clear();
            Assert.That(weaponCollection.Count, Is.EqualTo(0));

            Assert.That(weaponCollection.SaveCSV(csvOutputPath), Is.True);

            WeaponCollection output = new WeaponCollection();
            Assert.That(output.Load(csvOutputPath), Is.True);

            Assert.That(output.Count, Is.EqualTo(0));
        }

        // Load invalid format tests

        [Test]
        public void WeaponCollection_Load_SaveJSON_LoadXML_InvalidXml()
        {
            // Load CSV
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Save as JSON
            Assert.That(weaponCollection.SaveJSON(jsonOutputPath), Is.True);

            // Try to load JSON file as XML
            WeaponCollection output = new WeaponCollection();
            bool loaded = output.LoadXML(jsonOutputPath);
            Assert.That(loaded, Is.False);
            Assert.That(output.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_Load_SaveXML_LoadJSON_InvalidJson()
        {
            // Load CSV
            Assert.That(weaponCollection.Load(inputPath), Is.True);

            // Save as XML
            Assert.That(weaponCollection.SaveXML(xmlOutputPath), Is.True);

            // Try to load XML file as JSON
            WeaponCollection output = new WeaponCollection();
            bool loaded = output.LoadJSON(xmlOutputPath);
            Assert.That(loaded, Is.False);
            Assert.That(output.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadXML_InvalidXml()
        {
            // Create new collection and load CSV
            WeaponCollection output = new WeaponCollection();

            // Try to load CSV file as XML
            bool loaded = output.LoadXML(inputPath);

            Assert.That(loaded, Is.False);
            Assert.That(output.Count, Is.EqualTo(0));
        }

        [Test]
        public void WeaponCollection_ValidCsv_LoadJSON_InvalidJson()
        {
            // Create new collection and load CSV
            WeaponCollection output = new WeaponCollection();

            // Try to load CSV file as JSON
            bool loaded = output.LoadJSON(inputPath);

            Assert.That(loaded, Is.False);
            Assert.That(output.Count, Is.EqualTo(0));
        }
    }
}

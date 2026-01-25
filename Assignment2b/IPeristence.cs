using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Assignment2b
{
    public interface IPersistence
    {
        bool Load(string filename);
        bool Save(string filename);
    }

    public interface IXmlSerializable
    {
        bool LoadXML(string filename);
        bool SaveXML(string filename);
    }

    public interface IJsonSerializable
    {
        bool LoadJSON(string filename);
        bool SaveJSON(string filename);
    }

    public interface ICsvSerializable
    {
        bool LoadCSV(string filename);
        bool SaveCSV(string filename);
    }

}

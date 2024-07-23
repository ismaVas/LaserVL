using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;


namespace LaserVL.config
{
    [Serializable]
    public class LaserConfigs
    {
        public string IpAddress {  get; set; }
        public int Port {  get; set; }
        public int BaudRate {  get; set; }
        public int DataBit {  get; set; }
        public int StopBit { get; set; }
        public bool PartyBit { get; set; }
        public bool ControlFlow { get; set; }
        [XmlElement("Commands")]
        public Commands commands { get; set; }

        public static LaserConfigs LoadLaserConfigs(string configFile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LaserConfigs));
            using(FileStream fs = new FileStream(configFile, FileMode.Open, FileAccess.Read)) {
                return (LaserConfigs)serializer.Deserialize(fs);
            }
        }
    }

    [Serializable]
    public class Commands
    {
        [XmlElement("Command")]
        public Command[] CommandList { get; set; }
    }

    [Serializable]
    public class Command
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute] 
        public string Send { get; set; }
        [XmlAttribute]
        public string Return { get; set; }
    }

}

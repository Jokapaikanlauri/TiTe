using System;
namespace WeldingDataApplication.Classes
{
    public class Weld
    {
        public Weld()
        {
            

        }

        public class ResponseMetadata
        {
            public string Version { get; set; }
            public bool GetsDeprecated { get; set; }
            public List<object> Errors { get; set; }
        }

        public class Root
        {
            public List<WeldInfo> WeldInfos { get; set; }
            public ResponseMetadata ResponseMetadata { get; set; }
        }

        public class WeldInfo
        {
            public string Id { get; set; }
            public DateTime Timestamp { get; set; }
            public string ProcessingStepNumber { get; set; }
            public string PartSerialNumber { get; set; }
            public string PartArticleNumber { get; set; }
            public string MachineType { get; set; }
            public string MachineSerialNumber { get; set; }
            public string Details { get; set; }
            public string State { get; set; }
            public string Welder { get; set; }
        }

    }





}


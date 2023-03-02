namespace WeldingDataApplication.Classes
{
    public class WeldDetails
    {
        public WeldDetails()
        {

        }
        public class WeldDataStats
        {
            public double Max { get; set; }
            public double Mean { get; set; }
            public double Min { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
        }

        public class WeldDataLimitViolation
        {
            public string ValueType { get; set; }
            public string ViolationType { get; set; }
        }

        public class WeldDataJobInfo
        {
            public string Name { get; set; }
            public int Nr { get; set; }
            public int Revision { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime LastModified { get; set; }
            public string Details { get; set; }
        }

        public class WeldDataSection
        {
            public int Number { get; set; }
            public WeldDataJobInfo JobInfo { get; set; }
            public string Details { get; set; }
        }

        public class WeldDataSingleStat
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Unit { get; set; }
        }

        public class WeldData
        {
            public List<WeldDataStats> Stats { get; set; }
            public List<WeldDataLimitViolation> LimitViolations { get; set; }
            public List<WeldDataSection> Sections { get; set; }
            public List<WeldDataSingleStat> SingleStats { get; set; }
        }

        public class Error
        {
            public string ErrorCode { get; set; }
        }

        public class Change
        {
            public string WeldingId { get; set; }
            public string State { get; set; }
            public string User { get; set; }
            public string Explanation { get; set; }
            public DateTime TimestampUtc { get; set; }
            public string EventId { get; set; }
        }

        public class ResponseMetadataError
        {
            public int ErrorCode { get; set; }
            public string ErrorCodeName { get; set; }
            public string Description { get; set; }
        }

        public class ResponseMetadata
        {
            public string Version { get; set; }
            public bool GetsDeprecated { get; set; }
            public List<ResponseMetadataError> Errors { get; set; }
        }

        public class RootObject
        {
            public string WeldId { get; set; }
            public string PartItemNumber { get; set; }
            public string PartSerialNumber { get; set; }
            public string ProcessingStepNumber { get; set; }
            public string MachineSerialNumber { get; set; }
            public string MachineType { get; set; }
            public string Model { get; set; }
            public string Welder { get; set; }
            public DateTime TimeStamp { get; set; }
            public double Duration { get; set; }
            public WeldData WeldData { get; set; }
            public List<Error> Errors { get; set; }
            public bool IsCompleted { get; set; }
            public Dictionary<string, string> Units { get; set; }
            public string ActualValues { get; set; }
            public int ProgramNumber { get; set; }
            public string State { get; set; }
            public List<Change> Changes { get; set; }
            public ResponseMetadata ResponseMetadata { get; set; }
        }


    }
}

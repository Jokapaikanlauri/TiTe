namespace WeldingDataApplication.Classes
{
    public class WeldDetails
    {
        public WeldDetails()
        {

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

        public class Error
        {
            public string ErrorCode { get; set; }
            public string ErrorCodeName { get; set; }
            public string Description { get; set; }
        }

        public class JobInfo
        {
            public string Name { get; set; }
            public int Nr { get; set; }
            public int Revision { get; set; }
            public bool IsDeleted { get; set; }
            public DateTime LastModified { get; set; }
            public string Details { get; set; }
        }

        public class LimitViolation
        {
            public string ValueType { get; set; }
            public string ViolationType { get; set; }
        }

        public class ResponseMetadata
        {
            public string Version { get; set; }
            public bool GetsDeprecated { get; set; }
            public List<Error> Errors { get; set; }
        }

        public class Root
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
            public int Duration { get; set; }
            public WeldData WeldData { get; set; }
            public List<Error> Errors { get; set; }
            public bool IsCompleted { get; set; }
            public Units Units { get; set; }
            public string ActualValues { get; set; }
            public int ProgramNumber { get; set; }
            public string State { get; set; }
            public List<Change> Changes { get; set; }
            public ResponseMetadata ResponseMetadata { get; set; }
        }

        public class Section
        {
            public int Number { get; set; }
            public JobInfo JobInfo { get; set; }
            public string Details { get; set; }
        }

        public class SingleStat
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string Unit { get; set; }
        }

        public class Stat
        {
            public int Max { get; set; }
            public int Mean { get; set; }
            public int Min { get; set; }
            public string Name { get; set; }
            public string Unit { get; set; }
        }

        public class Units
        {
        }

        public class WeldData
        {
            public List<Stat> Stats { get; set; }
            public List<LimitViolation> LimitViolations { get; set; }
            public List<Section> Sections { get; set; }
            public List<SingleStat> SingleStats { get; set; }
        }


    }
}

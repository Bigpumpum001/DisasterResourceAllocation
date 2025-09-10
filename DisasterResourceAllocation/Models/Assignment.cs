namespace DisasterResourceAllocation.Models
{

    public class Assignment
    {
        public string AreaID { get; set; }
        public string TruckID { get; set; }
        public Dictionary<string, int> ResourcesDelivery { get; set; }
    }
    public class FailedAssignment
    {
        public string AreaID { get; set; }
        public List<string> Reason { get; set; }
    }
    public class ResultAssignment
    {
        public List<Assignment> Assignments { get; set; } = new List<Assignment>();
        public List<FailedAssignment> FailedAssignments { get; set; } = new List<FailedAssignment>();
    }
}

namespace LeaveService.Model
{
    public class Leave
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LeaveStart { get; set; }
        public DateTime LeaveEnd { get; set; }
        public string LeaveType { get; set; }
        public string Status { get; set; }
    }
}

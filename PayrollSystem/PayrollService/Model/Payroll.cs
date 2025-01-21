namespace PayrollService.Model
{
    public class Payroll
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal Allowance { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetSalary { get; set; }
        
    }
}

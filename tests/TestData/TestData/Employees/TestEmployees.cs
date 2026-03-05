namespace Linn.Stores2.TestData.Employees
{
    using Linn.Stores2.Domain.LinnApps;

    public class TestEmployees
    {
        // For privacy reasons these Employees are fictional and like
        // Pro Evolution football players, any resemblance to real persons,
        // living or dead, is purely coincidental.

        public static Employee SophlyBard => new Employee
        {
            Id = 33225,
            Name = "Sophly Bard",
            DepartmentCode = "PURCHASING"
        };

        public static Employee ColinODuty => new Employee
        {
            Id = 33212,
            Name = "Colin O'Duty",
            DepartmentCode = "PURCHASING"
        };
    }
}

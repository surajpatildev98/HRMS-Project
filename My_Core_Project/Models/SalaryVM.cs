namespace My_Core_Project.Models
{
    public class SalaryVM
    {
        public int SalaryId { get; set; }

        public int? EmployeeId { get; set; }

        public decimal? BasicPay { get; set; }

        public decimal? Hra { get; set; }

        public decimal? Da { get; set; }

        public decimal? Tax { get; set; }

        public decimal? NetPay { get; set; }

        public string? EmployeeName { get; set; } // for display only
        public string? DesignationName { get; set; } // for display only

        public DateOnly? EffectiveFrom { get; set; }

        // 🔹 Formatted properties (read-only)
        public string BasicPayFormatted => (BasicPay ?? 0).ToString("N2");
        public string HraFormatted => (Hra ?? 0).ToString("N2");
        public string DaFormatted => (Da ?? 0).ToString("N2");
        public string TaxFormatted => (Tax ?? 0).ToString("N2");
        public string NetPayFormatted => (NetPay ?? 0).ToString("N2");
    }
}

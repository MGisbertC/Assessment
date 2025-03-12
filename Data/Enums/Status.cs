using System.ComponentModel;

namespace MGisbert.Appointments.Data.Enums
{
    public enum Status
    {
        [Description("Pending")]
        Pending,
        [Description("Approved")]
        Approved,
        [Description("Cancelled")]
        Cancelled
    }
}

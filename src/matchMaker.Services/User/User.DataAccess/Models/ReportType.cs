namespace User.DataAccess.Models;

public class ReportType
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<UserReport> Reports { get; set; } = new List<UserReport>();
}
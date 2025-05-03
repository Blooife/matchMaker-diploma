using System.ComponentModel;

namespace Common.Enums;

public enum ReportStatus
{
    [Description("В обработке")]
    Pending = 0,
    
    [Description("Обработана")]
    Reviewed = 1,
    
    [Description("Отклонена")]
    Rejected = 2,
    
    [Description("Пользователь заблокирован")]
    Blocked = 3, 
}
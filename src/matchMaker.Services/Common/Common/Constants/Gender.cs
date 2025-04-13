using System.ComponentModel;

namespace Common.Constants;

public enum Gender
{
    [Description("Мужчина")]
    Male = 1,
    
    [Description("Женщина")]
    Female = 2,
    
    [Description("Не определено")]
    Undefined = 3
}
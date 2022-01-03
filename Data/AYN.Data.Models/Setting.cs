using AYN.Data.Common.Models;

namespace AYN.Data.Models;

public class Setting : BaseDeletableModel<int>
{
    public string Name { get; set; }

    public string Value { get; set; }
}

using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Administration.Chat;

public class BlacklistWordViewModel : IMapFrom<WordBlacklist>
{
    public int Id { get; set; }

    public string Content { get; set; }
}

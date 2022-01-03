using AYN.Data.Models;
using AYN.Services.Mapping;

namespace AYN.Web.ViewModels.Addresses;

public class GetAddressesViewModel : IMapFrom<Address>
{
    public int Id { get; set; }

    public string Name { get; set; }
}

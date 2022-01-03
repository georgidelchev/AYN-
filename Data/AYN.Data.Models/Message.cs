using AYN.Data.Common.Models;

namespace AYN.Data.Models;

public class Message : BaseDeletableModel<int>
{
    public string SenderId { get; set; }

    public ApplicationUser Sender { get; set; }

    public string ReceiverId { get; set; }

    public ApplicationUser Receiver { get; set; }

    public string Content { get; set; }

    public bool IsRead { get; set; }
}

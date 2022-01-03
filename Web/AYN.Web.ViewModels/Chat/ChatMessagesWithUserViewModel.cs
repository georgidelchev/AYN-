using System;

using AYN.Data.Models;
using AYN.Services.Mapping;
using Ganss.XSS;

namespace AYN.Web.ViewModels.Chat;

public class ChatMessagesWithUserViewModel : IMapFrom<Message>
{
    private readonly IHtmlSanitizer sanitizer;

    public ChatMessagesWithUserViewModel()
    {
        this.sanitizer = new HtmlSanitizer();
    }

    public string Content { get; set; }

    public string SanitizedContent
        => this.sanitizer.Sanitize(this.Content);

    public string SenderId { get; set; }

    public string SenderUserName { get; set; }

    public string SenderAvatarImageUrl { get; set; }

    public DateTime CreatedOn { get; set; }
}

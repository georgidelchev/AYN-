using System.Threading.Tasks;

using AYN.Services.Data.Interfaces;
using AYN.Services.Messaging;
using AYN.Web.Infrastructure.Extensions;
using AYN.Web.ViewModels.Feedback;
using Microsoft.AspNetCore.Mvc;

namespace AYN.Web.Controllers;

public class FeedbackController : BaseController
{
    private readonly IFeedbackService feedbackService;
    private readonly IEmailSender emailSender;

    public FeedbackController(
        IFeedbackService feedbackService,
        IEmailSender emailSender)
    {
        this.feedbackService = feedbackService;
        this.emailSender = emailSender;
    }

    /// <summary>
    /// Create GET Method -> Returns the view with forms to create Feedback.
    /// </summary>
    /// <returns>Create Feedback View with filled email.</returns>
    [HttpGet]
    public IActionResult Create()
        => this.View(new CreateFeedbackInputModel
        {
            Email = this.User.GetEmail(),
        });

    /// <summary>
    /// Create POST Method -> Creates feedback with given Input Model and sends thanks email to the user.
    /// </summary>
    /// <param name="input">input model to create feedback.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateFeedbackInputModel input)
    {
        await this.feedbackService.CreateAsync(input, this.User.GetId());

        await this.emailSender.SendEmailAsync(
            from: "allyouneedplatform@gmail.com",
            fromName: "AYNPlatform",
            to: input.Email,
            subject: "Thanks for your Feedback",
            htmlContent: $"Thanks for your feedback! <br/> <strong>{input.Title}</strong><br/>{input.Content}");

        return this.Redirect("/");
    }
}

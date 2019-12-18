using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples
{
    public class FeedbackPrompt : Prompt<bool>
    {
        public FeedbackPrompt(string dialogId) : base(dialogId)
        {
        }
        protected override async Task OnPromptAsync(ITurnContext turnContext, IDictionary<string, object> state, PromptOptions options, bool isRetry, CancellationToken cancellationToken = default(CancellationToken))
        {
            var reply = turnContext.Activity.CreateReply();
            reply.Attachments = new List<Attachment>
            {
                new HeroCard
                {
                    Text = options.Prompt.Text,
                    Buttons = new List<CardAction>
                    {
                        new CardAction { Title = "Yes", Type = "imBack", Value = "Yes" },
                        new CardAction { Title = "No", Type = "imBack", Value = "No" },
                    },
                }.ToAttachment(),
            };
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
        protected override Task<PromptRecognizerResult<bool>> OnRecognizeAsync(ITurnContext turnContext, IDictionary<string, object> state, PromptOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            switch (turnContext.Activity.Text.Trim())
            {
                case "Yes":
                    return Task.FromResult(new PromptRecognizerResult<bool>
                    {
                        Succeeded = true,
                        Value = true,
                    });
                case "No":
                    return Task.FromResult(new PromptRecognizerResult<bool>
                    {
                        Succeeded = true,
                        Value = false,
                    });
                default:
                    return Task.FromResult(new PromptRecognizerResult<bool>
                    {
                        Succeeded = false,
                    });
            }
        }
    }
}
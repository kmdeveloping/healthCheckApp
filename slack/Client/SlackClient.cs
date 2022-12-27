using Microsoft.Extensions.Logging;
using slack.Config;
using Slack.Webhooks;

namespace slack.Client;

public class Slack : ISlack
{
  private readonly ILogger<SlackClient> _logger;
  private readonly SlackClient _slackClient;
  private readonly SlackMessage _slackMessage;
  private readonly List<SlackAttachment> _slackAttachmentList;
  private List<SlackField> _slackFieldsList;
  private static string _emoji, _msgColor, webook;

  public Slack(SlackConfiguration slackConfiguration, ILogger<SlackClient> logger)
  {
    _logger = logger;
    _slackClient = new SlackClient(slackConfiguration.WebhookUrl);
    _slackMessage = new SlackMessage();
    _slackAttachmentList = new List<SlackAttachment>();
    _slackFieldsList = new List<SlackField>();
    webook = slackConfiguration.WebhookUrl; 
  }

  public async Task Testing()
  {
    _logger.LogInformation("Slack method called {Method}", nameof(Testing));
    Console.WriteLine($"Slack Testing Implemented with webhook {webook}");
  }
}
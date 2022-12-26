using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

  public Slack(IOptions<SlackConfiguration> config, ILogger<SlackClient> logger)
  {
    _logger = logger;
    _slackClient = new SlackClient(config.Value.WebhookUrl);
    _slackMessage = new SlackMessage();
    _slackAttachmentList = new List<SlackAttachment>();
    _slackFieldsList = new List<SlackField>();
    webook = config.Value.WebhookUrl;
  }

  public async Task Testing()
  {
    _logger.LogInformation("Slack method called {Method}", nameof(Testing));
    Console.WriteLine($"Slack Testing Implemented with webhook {webook}");
  }
}
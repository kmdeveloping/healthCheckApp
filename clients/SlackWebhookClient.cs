using clients.Configuration;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;

namespace clients;

public interface ISlackWebhookClient
{
  Task Testing();
}

public class SlackWebhookClient : ISlackWebhookClient
{
  private readonly ILogger<SlackWebhookClient> _logger;
  private readonly SlackClient _slackClient;
  private readonly SlackMessage _slackMessage;
  private readonly List<SlackAttachment> _slackAttachmentList;
  private List<SlackField> _slackFieldsList;
  private string _webook;
  private string _emoji;
  private string _msgColor;

  public SlackWebhookClient(SlackClientConfiguration slackClientConfiguration, ILogger<SlackWebhookClient> logger)
  {
    _logger = logger;
    _slackClient = new SlackClient(slackClientConfiguration.WebhookUrl);
    _slackMessage = new SlackMessage();
    _slackAttachmentList = new List<SlackAttachment>();
    _slackFieldsList = new List<SlackField>();
    
    _webook = slackClientConfiguration.WebhookUrl; 
  }
  
  public Task Testing()
  {
    _logger.LogInformation("Slack method called {Method}", nameof(Testing));
    Console.WriteLine($"Slack Testing Implemented with webhook {_webook}");
    return Task.CompletedTask;
  }
}
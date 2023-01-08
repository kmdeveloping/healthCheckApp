using clients.Configuration;
using clients.Models;
using Microsoft.Extensions.Logging;
using Slack.Webhooks;
using Exception = System.Exception;

namespace clients;

public interface IHealthCheckSlackClient
{
  Task SendAsync(SlackMessageEnum messageType, string message);
}

public class HealthCheckSlackClient : IHealthCheckSlackClient
{
  private readonly ILogger<HealthCheckSlackClient> _logger;
  private readonly SlackClient _slackClient;
  
  private SlackMessage _slackMessage;
  private List<SlackField> _slackFieldsList;
  private string _emoji;
  private string _msgColor;

  public HealthCheckSlackClient(IClientConfiguration<HealthCheckSlackClient> configuration, ILogger<HealthCheckSlackClient> logger)
  {
    _logger = logger;
    _slackClient = new SlackClient(configuration.DestinationUri);
  }
  
  public async Task SendAsync(SlackMessageEnum messageType, string message)
  {
    _slackMessage = new SlackMessage();
    _slackFieldsList = new List<SlackField>();
    
    await CreateFields(messageType, message);
    await CreateAttachment(messageType);

    try
    {
      await _slackClient.PostAsync(_slackMessage);
    }
    catch (Exception ex)
    {
      _logger.LogError("ERROR: {ErrorMessage}", ex.Message);
    }
  }

  private async Task CreateAttachment(SlackMessageEnum messageType)
  {
    await EmojiAndColorMapper(messageType);
    
    _slackMessage.Attachments= new List<SlackAttachment>
    {
      new SlackAttachment
      {
        Title = $"{_emoji} Network Scan Notice",
        Color = _msgColor,
        Fields = _slackFieldsList
      }
    };
  }

  private Task CreateFields(SlackMessageEnum messageType, string message)
  {
    SlackField titleField = new SlackField
    {
      Short = false
    };

    SlackField messageField = new SlackField
    {
      Title = "Message: ",
      Short = true
    };

    SlackField networkStateField = new SlackField
    {
      Title = "State: ",
      Short = true
    };

    switch (messageType)
    {
      case SlackMessageEnum.RedisClientError:
        titleField.Title = "Error on redis client";
        messageField.Value = message;
        networkStateField.Value = "Unknown";
        break;
      case SlackMessageEnum.NetworkStatusError:
        titleField.Title = "Network status error";
        messageField.Value = message;
        networkStateField.Value = "Down";
        break;
      case SlackMessageEnum.NetworkStatusRestored:
        titleField.Title = "Network restored";
        messageField.Value = message;
        networkStateField.Value = "Up";
        break;
      case SlackMessageEnum.UnknownError:
        titleField.Title = "Unexpected Error";
        messageField.Value = message;
        networkStateField.Value = "Unknown";
        break;
    }

    _slackFieldsList.Add(titleField);
    _slackFieldsList.Add(messageField);
    _slackFieldsList.Add(networkStateField);
    
    return Task.CompletedTask;
  }

  private Task EmojiAndColorMapper(SlackMessageEnum messageType)
  {
    _emoji = messageType switch
    {
      SlackMessageEnum.NetworkStatusError => ":alert:",
      SlackMessageEnum.RedisClientError => ":alert:",
      SlackMessageEnum.NetworkStatusRestored => ":boom:",
      _ => ":alert:"
    };

    _msgColor = messageType switch
    {
      SlackMessageEnum.NetworkStatusError => "#Ffa500",
      SlackMessageEnum.RedisClientError => "#FF0000",
      SlackMessageEnum.NetworkStatusRestored => "#33effc",
      _ => "#FF0000"
    };
    
    return Task.CompletedTask;
  }
}
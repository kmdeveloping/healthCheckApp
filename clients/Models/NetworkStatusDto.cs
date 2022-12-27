using Newtonsoft.Json;

namespace clients.Models;

public class NetworkStatusDto
{
  [JsonProperty("StatusCode")]
  public int StatusCode { get; set; }
  [JsonProperty("Status")]
  public string StatusMessage { get; set; }
  [JsonProperty("ResponseHeaders")]
  public string ResponseHeader { get; set; }
}
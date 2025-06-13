using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pomnesh.Application.Interfaces;
using Pomnesh.API.Dto;
using Pomnesh.API.Responses;
using System.Security.Claims;
using System.Text.Json.Nodes;
using VkNet;
using VkNet.Model;
using VkNet.Enums.StringEnums;
using VkNet.Exception;
using Newtonsoft.Json;
using VkNet.Utils;
using System.Linq;
using Newtonsoft.Json.Linq;
using Pomnesh.Domain.Enum;
using Microsoft.Extensions.Logging;

namespace Pomnesh.API.Controllers;

[Route("api/v1/vk")]
[ApiController]
[Authorize]
public class VkController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<VkController> _logger;
    private const int DefaultCount = 20;
    private const int MaxCount = 200;

    public VkController(IUserService userService, ILogger<VkController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("getUserChats")]
    public async Task<IActionResult> GetUserChats([FromQuery] int? offset = null, [FromQuery] int? count = null)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new BaseApiResponse<string> { Error = "User not found" });
        }

        var user = await _userService.Get(long.Parse(userId));
        if (user == null || string.IsNullOrEmpty(user.VkToken))
        {
            return BadRequest(new BaseApiResponse<string> { Error = "VkToken not found" });
        }

        try
        {
            var api = new VkApi();
            await api.AuthorizeAsync(new ApiAuthParams { AccessToken = user.VkToken });
            
            var parameters = new VkParameters
            {
                { "count", Math.Min(count ?? DefaultCount, MaxCount) },
                { "offset", offset ?? 0 },
                { "filter", "all" },
                { "extended", 1 },
                { "fields", "photo_50,first_name,last_name" }
            };

            var response = await api.CallAsync("messages.getConversations", parameters);
            
            if (response == null || !response.ContainsKey("items") || !response.ContainsKey("count"))
            {
                return Ok(new BaseApiResponse<object> { 
                    Payload = new {
                        Items = Array.Empty<object>(),
                        TotalCount = 0,
                        Offset = offset ?? 0,
                        Count = Math.Min(count ?? DefaultCount, MaxCount)
                    }
                });
            }

            var items = JsonConvert.DeserializeObject<JArray>(response["items"].ToString());
            var profiles = response.ContainsKey("profiles") 
                ? JsonConvert.DeserializeObject<JArray>(response["profiles"].ToString())
                : new JArray();
            var groups = response.ContainsKey("groups")
                ? JsonConvert.DeserializeObject<JArray>(response["groups"].ToString())
                : new JArray();
            var totalCount = JsonConvert.DeserializeObject<int>(response["count"].ToString());
            
            var result = items.Select(item => {
                var conversation = item["conversation"] as JObject;
                var peer = conversation?["peer"] as JObject;
                var lastMessage = item["last_message"] as JObject;
                var chatSettings = conversation?["chat_settings"] as JObject;
                
                var peerId = peer?["id"]?.Value<long>() ?? 0;
                var peerType = peer?["type"]?.Value<string>();
                var isGroupChat = peerType == "chat";
                
                string name = null;
                string photoUrl = null;
                
                if (isGroupChat)
                {
                    name = chatSettings?["title"]?.Value<string>();
                    photoUrl = chatSettings?["photo"]?["photo_50"]?.Value<string>();
                }
                else if (peerType == "user")
                {
                    var profile = profiles.FirstOrDefault(p => p["id"].Value<long>() == peerId) as JObject;
                    if (profile != null)
                    {
                        name = $"{profile["first_name"]?.Value<string>()} {profile["last_name"]?.Value<string>()}".Trim();
                        photoUrl = profile["photo_50"]?.Value<string>();
                    }
                }
                else if (peerType == "group")
                {
                    var group = groups.FirstOrDefault(g => g["id"].Value<long>() == Math.Abs(peerId)) as JObject;
                    if (group != null)
                    {
                        name = group["name"]?.Value<string>();
                        photoUrl = group["photo_50"]?.Value<string>();
                    }
                }
                
                return new {
                    Id = peerId,
                    Name = name,
                    PhotoUrl = photoUrl,
                    Type = peerType,
                    LastMessage = lastMessage?["text"]?.Value<string>(),
                    UnreadCount = conversation?["unread_count"]?.Value<int>() ?? 0,
                    IsGroupChat = isGroupChat
                };
            });

            return Ok(new BaseApiResponse<object> { 
                Payload = new {
                    Items = result,
                    TotalCount = totalCount,
                    Offset = offset ?? 0,
                    Count = Math.Min(count ?? DefaultCount, MaxCount)
                }
            });
        }
        catch (VkApiException ex)
        {
            return BadRequest(new BaseApiResponse<string> { Error = $"VK API error: {ex.Message}" });
        }
        catch (JsonReaderException ex)
        {
            return BadRequest(new BaseApiResponse<string> { Error = $"Error parsing VK response: {ex.Message}" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new BaseApiResponse<string> { Error = $"Error: {ex.Message}" });
        }
    }

    [HttpGet("getAttachments")]
    public async Task<IActionResult> GetAttachments(
        [FromQuery] long? peerId = null,
        [FromQuery] string startFrom = null,
        [FromQuery] int? count = null,
        [FromQuery] AttachmentType[] types = null,
        [FromQuery] bool includeForwards = true)
    {
        if (!peerId.HasValue)
        {
            return BadRequest(new BaseApiResponse<string> { Error = "Parameter 'peerId' is required" });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new BaseApiResponse<string> { Error = "User not found" });
        }

        var user = await _userService.Get(long.Parse(userId));
        if (user == null || string.IsNullOrEmpty(user.VkToken))
        {
            return BadRequest(new BaseApiResponse<string> { Error = "VkToken not found" });
        }

        try
        {
            var api = new VkApi();
            await api.AuthorizeAsync(new ApiAuthParams { AccessToken = user.VkToken });

            var parameters = new VkParameters
            {
                { "peer_id", peerId.Value },
                { "count", Math.Min(count ?? DefaultCount, MaxCount) },
                { "max_forwards_level", includeForwards ? 45 : 0 },
                { "extended", 1 }
            };

            if (!string.IsNullOrEmpty(startFrom))
            {
                parameters.Add("start_from", startFrom);
            }

            if (types != null && types.Length > 0)
            {
                var mediaTypes = new List<string>();
                foreach (var type in types)
                {
                    switch (type)
                    {
                        case AttachmentType.Photo:
                            mediaTypes.Add("photo");
                            break;
                        case AttachmentType.Video:
                            mediaTypes.Add("video");
                            break;
                        case AttachmentType.Audio:
                            mediaTypes.Add("audio");
                            break;
                        case AttachmentType.AudioMessage:
                            mediaTypes.Add("audio_message");
                            break;
                        case AttachmentType.Document:
                            mediaTypes.Add("doc");
                            break;
                    }
                }
                if (mediaTypes.Count > 0)
                {
                    parameters.Add("media_type", string.Join(",", mediaTypes));
                }
            }

            var response = await api.CallAsync("messages.getHistoryAttachments", parameters);
            
            if (response == null || string.IsNullOrEmpty(response.RawJson))
            {
                return Ok(new BaseApiResponse<object> { 
                    Payload = new {
                        Items = Array.Empty<object>(),
                        TotalCount = 0,
                        NextFrom = "",
                        Count = Math.Min(count ?? DefaultCount, MaxCount)
                    }
                });
            }

            var responseData = JsonConvert.DeserializeObject<JObject>(response.RawJson);
            if (responseData == null || !responseData.ContainsKey("response"))
            {
                return Ok(new BaseApiResponse<object> { 
                    Payload = new {
                        Items = Array.Empty<object>(),
                        TotalCount = 0,
                        NextFrom = "",
                        Count = Math.Min(count ?? DefaultCount, MaxCount)
                    }
                });
            }

            var responseObj = responseData["response"] as JObject;
            if (responseObj == null || !responseObj.ContainsKey("items"))
            {
                return Ok(new BaseApiResponse<object> { 
                    Payload = new {
                        Items = Array.Empty<object>(),
                        TotalCount = 0,
                        NextFrom = "",
                        Count = Math.Min(count ?? DefaultCount, MaxCount)
                    }
                });
            }

            var items = responseObj["items"] as JArray;
            if (items == null)
            {
                return Ok(new BaseApiResponse<object> { 
                    Payload = new {
                        Items = Array.Empty<object>(),
                        TotalCount = 0,
                        NextFrom = "",
                        Count = Math.Min(count ?? DefaultCount, MaxCount)
                    }
                });
            }

            var nextFrom = responseObj["next_from"]?.Value<string>() ?? "";
            
            var result = items.Select(item => {
                var attachment = item["attachment"] as JObject;
                var type = attachment?["type"]?.Value<string>();
                var attachmentData = attachment?[type] as JObject;
                
                object attachmentInfo = null;
                switch (type)
                {
                    case "photo":
                        var sizes = attachmentData?["sizes"] as JArray;
                        var maxSize = sizes?.OrderByDescending(s => s["width"].Value<int>()).FirstOrDefault() as JObject;
                        attachmentInfo = new {
                            Url = maxSize?["url"]?.Value<string>(),
                            Width = maxSize?["width"]?.Value<int>(),
                            Height = maxSize?["height"]?.Value<int>(),
                            Id = attachmentData?["id"]?.Value<long>(),
                            OwnerId = attachmentData?["owner_id"]?.Value<long>(),
                            AccessKey = attachmentData?["access_key"]?.Value<string>()
                        };
                        break;
                    case "video":
                        attachmentInfo = new {
                            Title = attachmentData?["title"]?.Value<string>(),
                            Description = attachmentData?["description"]?.Value<string>(),
                            Duration = attachmentData?["duration"]?.Value<int>(),
                            PhotoUrl = attachmentData?["photo_320"]?.Value<string>(),
                            Id = attachmentData?["id"]?.Value<long>(),
                            OwnerId = attachmentData?["owner_id"]?.Value<long>(),
                            AccessKey = attachmentData?["access_key"]?.Value<string>()
                        };
                        break;
                    case "audio":
                        attachmentInfo = new {
                            Artist = attachmentData?["artist"]?.Value<string>(),
                            Title = attachmentData?["title"]?.Value<string>(),
                            Duration = attachmentData?["duration"]?.Value<int>(),
                            Url = attachmentData?["url"]?.Value<string>(),
                            Id = attachmentData?["id"]?.Value<long>(),
                            OwnerId = attachmentData?["owner_id"]?.Value<long>()
                        };
                        break;
                    case "audio_message":
                        attachmentInfo = new {
                            Duration = attachmentData?["duration"]?.Value<int>(),
                            Waveform = attachmentData?["waveform"]?.Value<int[]>(),
                            LinkOgg = attachmentData?["link_ogg"]?.Value<string>(),
                            LinkMp3 = attachmentData?["link_mp3"]?.Value<string>(),
                            Id = attachmentData?["id"]?.Value<long>(),
                            OwnerId = attachmentData?["owner_id"]?.Value<long>(),
                            AccessKey = attachmentData?["access_key"]?.Value<string>()
                        };
                        break;
                    case "doc":
                        attachmentInfo = new {
                            Title = attachmentData?["title"]?.Value<string>(),
                            Size = attachmentData?["size"]?.Value<long>(),
                            Ext = attachmentData?["ext"]?.Value<string>(),
                            Url = attachmentData?["url"]?.Value<string>(),
                            Id = attachmentData?["id"]?.Value<long>(),
                            OwnerId = attachmentData?["owner_id"]?.Value<long>(),
                            AccessKey = attachmentData?["access_key"]?.Value<string>()
                        };
                        break;
                }
                
                return new {
                    Type = type,
                    AttachmentInfo = attachmentInfo,
                    MessageId = item["message_id"]?.Value<long>(),
                    FromId = item["from_id"]?.Value<long>(),
                    Date = item["date"]?.Value<long>()
                };
            });

            return Ok(new BaseApiResponse<object> { 
                Payload = new {
                    Items = result,
                    NextFrom = nextFrom,
                    Count = Math.Min(count ?? DefaultCount, MaxCount)
                }
            });
        }
        catch (VkApiException ex)
        {
            _logger.LogError(ex, "VK API error occurred");
            return BadRequest(new BaseApiResponse<string> { Error = $"VK API error: {ex.Message}" });
        }
        catch (JsonReaderException ex)
        {
            _logger.LogError(ex, "Error parsing VK response");
            return BadRequest(new BaseApiResponse<string> { Error = $"Error parsing VK response: {ex.Message}" });
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");
            return BadRequest(new BaseApiResponse<string> { Error = $"Error: {ex.Message}" });
        }
    }
} 
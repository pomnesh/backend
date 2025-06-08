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

namespace Pomnesh.API.Controllers;

[Route("api/v1/vk")]
[ApiController]
[Authorize]
public class VkController : ControllerBase
{
    private readonly IUserService _userService;
    private const int DefaultCount = 20;
    private const int MaxCount = 200;

    public VkController(IUserService userService)
    {
        _userService = userService;
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
} 
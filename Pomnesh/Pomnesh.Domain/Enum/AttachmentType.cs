using System.ComponentModel.DataAnnotations;

namespace Pomnesh.Domain.Enum;

public enum AttachmentType
{
    [Display(Name = "Photo")]
    Photo = 0,
    [Display(Name = "Video")]
    Video = 1,
    [Display(Name = "Audio")]
    Audio = 2,
    [Display(Name = "AudioMessage")]
    AudioMessage = 3,
    [Display(Name = "Document")]
    Document = 4,
    [Display(Name = "Link")]  // I think it's useless for this project
    Link = 5,
    [Display(Name = "Market")]
    Market = 6,
    [Display(Name = "Document")]
    MarketAlbum = 7,
    [Display(Name = "Wall")]
    Wall = 8,
    [Display(Name = "WallReply")]
    WallReply = 9,
    [Display(Name = "Sticker")]
    Sticker = 10,
    [Display(Name = "GiftItem")]
    GiftItem = 11,
    [Display(Name = "Call")]
    Call = 12
}
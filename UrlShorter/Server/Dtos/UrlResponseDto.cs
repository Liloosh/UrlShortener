using Server.Enums;
using Server.Models;

namespace Server.Dtos
{
    public class UrlResponseDto
    {
        public UrlEnum Response { get; set; }
        public Url? Url { get; set; }
    }
}

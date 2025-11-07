using System.Linq;

namespace Loudspeaker.ApiClients;

public partial class MessageV2
{
    [Newtonsoft.Json.JsonIgnore]
    public Contact? CreatorContact { get; set; }

    [Newtonsoft.Json.JsonIgnore]
    public string? CreatorDisplayName
    {
        get
        {
            if (CreatorContact != null)
            {
                var first = CreatorContact.First_name?.Trim();
                var last = CreatorContact.Last_name?.Trim();

                var combined = string.Join(" ", new[] { first, last }.Where(s => !string.IsNullOrWhiteSpace(s)));
                if (!string.IsNullOrWhiteSpace(combined))
                {
                    return combined;
                }
            }

            return string.IsNullOrWhiteSpace(Creator_id) ? null : Creator_id;
        }
    }
}



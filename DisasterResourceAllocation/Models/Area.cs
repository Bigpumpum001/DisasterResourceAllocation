using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DisasterResourceAllocation.Models
{
    [Table("areas")]
    public class Area
    {
        [Column("area_id")]
        [JsonPropertyName("AreaID")]
        public string AreaID { get; set; }

        [Column("urgency_level")]
        [JsonPropertyName("UrgencyLevel")]
        public int UrgencyLevel { get; set; }

        [Column("required_resources")]
        [JsonPropertyName("RequiredResources")]
        public Dictionary<string, int> RequiredResources { get; set; }
        
        [Column("time_constraint")]
        [JsonPropertyName("TimeConstraint")]
        public int TimeConstraint { get; set; }

    }
}

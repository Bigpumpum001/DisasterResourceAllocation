using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DisasterResourceAllocation.Models
{
    [Table("trucks")]
    public class Truck
    {
        [Column("truck_id")]
        [JsonPropertyName("TruckID")]
        public string TruckID { get; set; }

        [Column("available_resources")]
        [JsonPropertyName("AvailableResources")]
        public Dictionary<string, int> AvailableResources { get; set; }
        
        [Column("travel_time_to_area")]
        [JsonPropertyName("TravelTimeToArea")]
        public Dictionary<string, int> TravelTimeToArea { get; set; } 

    }
}

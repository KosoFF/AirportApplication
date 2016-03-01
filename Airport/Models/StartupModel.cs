using System.Runtime.Serialization;

namespace Airport.Models
{
    [DataContract]
    public class StartupModel
    {
        [DataMember(Name = "this")]
        public string This { get; set; }

        [DataMember(Name = "that")]
        public string That { get; set; }
    }
}
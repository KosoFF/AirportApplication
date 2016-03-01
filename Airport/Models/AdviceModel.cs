using System.Runtime.Serialization;

namespace Airport.Models
{
    public class AdviceWrapper
    {
        public AdviceModel Slip { get; set; }
    }

    [DataContract]
    public class AdviceModel
    {
        [DataMember(Name = "slip_id")]
        public int Id { get; set; }

        [DataMember(Name = "advice")]
        public string Text { get; set; }
    }
}
namespace Animal_Rescue.Models
{
    public class SearchByName
    {
        public int ID { get; set; }
        public string PrefixedID { get; set; }
        public string Species { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Spayed { get; set; }
        public string Breed { get; set; }
        public string Colour { get; set; }
        public DateTime? Birthday { get; set; }
        public string Vaccine_Status { get; set; }
        public string Identification { get; set; }
        public int? IdentificationNumber { get; set; }
        public int? Adoption_fee { get; set; }
    }
}

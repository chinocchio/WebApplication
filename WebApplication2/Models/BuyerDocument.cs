namespace WebApplication2.Models
{
    public class BuyerDocument
    {
        public int BuyerDocumentId { get; set; }
        public string? SPA { get; set; }
        public string? SPA_ACN { get; set; }
        public DateOnly DOR { get; set; }
        public string? BirthCertificate { get; set; }
        public string? Cenomar { get; set; }
        public string? MarriageCertificate { get; set; }
        public string? DeathCertificate { get; set; }
        public string? ProofOfBilling1 { get; set; }
        public string? VerifiedTin { get; set; }
        public string? TinAuthoLetter { get; set; }
        public string? Pb1904Form { get; set; }
        public string? Cb1904Form { get; set; }
        public string? TinNumber { get; set; }
        public string? PbIDType { get; set; }
        public string? ExpirationDate1 { get; set; }
        public string? SpsCbId { get; set; }
        public string? ExpirationDate2 { get; set; }
        public string? SpecificIncomeDocsSubmitted { get; set; }
        public string? AIFId { get; set; }
        public string? NameOfAif { get; set; }
        public string? ContactNumberOfAIF { get; set; }
        public string? ProofOfBilling2 { get; set; }
        public string? AdditionalPOI { get; set; }
        public string? OathOfAllegiance { get; set; }
        public List<SalesTransaction>? SalesTransaction { get; set; }
}
}

using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class SubmittedDocument
    {
        [Key]
        public int Id { get; set; }

        public string? DocumentCode { get; set; }

        public long? ContractNumber { get; set; }

        public string? CustomerCode { get; set; }

        public string? UnitCode { get; set; }

        public DateTime? DateSubmitted { get; set; }

        public string? DocumentName { get; set; }

    }
} 
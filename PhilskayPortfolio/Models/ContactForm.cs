using System.ComponentModel.DataAnnotations;

namespace PhilskayPortfolio.Models
{
    public class ContactForm
    {
        [Required]  
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string From { get; set; }    
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BudgetLimit
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Felső határ")]
        public int Limit { get; set; }

        [Required]
        [Display(Name = "Kezdőnap")]
        public DateTime StartDate { get; set; }

        [NotMapped]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Havi Limit?")]
        public bool IsMonthly { get; set; }
    }
}

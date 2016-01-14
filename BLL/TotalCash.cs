using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TotalCash
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Teljes összeg")]
        public int Amount { get; set; }

        [Required]
        [Display(Name = "Nap")]
        public DateTime Date { get; set; }
    }
}

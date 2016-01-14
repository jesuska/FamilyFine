using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace BLL
{
    public class Transaction
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name="Összeg")]
        [Required]
        public int Amount { get; set; }

        [Display(Name = "Nap")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Havi limit")]
        public bool AffectsMonthlyLimit { get; set; }

        [Display(Name="Speciális limit")]
        public bool AffectsSpecialLimit { get; set; }

        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [MaxLength(100)]
        [Display(Name = "Megjegyzés")]
        public string Comment { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as Transaction;

            if (item == null)
            {
                return false;
            }

            return this.Id.Equals(item.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}

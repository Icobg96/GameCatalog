using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog.Data.Entities
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "Rating")]
        public string RatingValue { get; set; }
        [Required]
        [StringLength(400, MinimumLength = 1)]
        public string Description { get; set; }
        public string GreatYear { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}

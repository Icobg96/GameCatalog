using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog.Data.Entities
{
   public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        [Display(Name = "Genre name")]
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Game> Games { get; set; }
    }
}

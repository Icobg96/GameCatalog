using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCatalog.Data.Entities
{
   public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; }
        [Required]
        [Display(Name= "Release Year")]
        public string ReleaseYear { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public int RatingId { get; set; }
        public virtual Rating Rating { get; set; }
        [Required]
        public string TrailerLink { get; set; }
    }
}

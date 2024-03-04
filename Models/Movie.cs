using System.ComponentModel.DataAnnotations;

namespace FilemSite.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required,MaxLength(250)]
        public string Title { get; set; }=string.Empty; 
        [Required,MaxLength(2500)]
        public string StoryLine { get; set; } = string.Empty;
        public int Year { get; set; }
        public double Rate { get; set; }
        public byte genreId { get; set; }
        [Required]
        public byte[] Poster { get; set; }
        public Genre genre { get; set; }
    }
}

using FilemSite.Models;
using System.ComponentModel.DataAnnotations;

namespace FilemSite.ViewModels
{
    public class ViewFormCreat
    {
        public int Id { get; set; }
        [Required, StringLength(250)]
        public string Title { get; set; } = string.Empty;
        [Required, StringLength(2500)]
        public string StoryLine { get; set; } = string.Empty;
        public int Year { get; set; }
        [Range(1,10)]
        public double Rate { get; set; }
        [Display(Name ="Genre")]
        public byte genreId { get; set; }
        [Required,Display(Name ="Select Poster")]
        public byte[] Poster { get; set; }
        public IEnumerable< Genre >genre { get; set; }
    }
}

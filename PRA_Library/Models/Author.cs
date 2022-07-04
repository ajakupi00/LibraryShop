using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Author
    {
        public int IDAuthor { get; set; }
        [Required(ErrorMessage = "Please write the authors full name")]
        [Display(Name = "Full name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Please write a description about the author")]
        public string Description { get; set; }
        [Display(Name = "Picture path")]
        [Required(ErrorMessage = "Please enter picture path of author")]
        public string PicturePath { get; set; }
    }
}

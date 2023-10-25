using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
namespace BootcampDay1.Models
{
    public class ProfileViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Profession { get; set; }

        public IFormFile? ProfileImage { get; set; }  // Yeni fotoğraf yüklemek için
        public string? CurrentImagePath { get; set; } // Var olan fotoğrafı göstermek için
    }
}


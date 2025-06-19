using System;
using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Başlık boş olamaz")]
    public string Title { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Kullanıcıya özel veri için eklenen alan:
    public string? UserId { get; set; }
}



namespace BootcampDay1.DTOs
{
    public class TodoItemDto
    {
        public int Id { get; set; }             // Güncelleme/silme için gerekli
        public string Title { get; set; } = ""; // Görev başlığı
        public bool IsCompleted { get; set; }   // Tamamlandı mı?
    }
}
// Bu DTO, API'den gelen TodoItem verilerini temsil eder.
// İstemciden gelen verileri alırken ve istemciye gönderirken kullanılır.
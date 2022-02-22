using System.ComponentModel.DataAnnotations;

namespace MonitorTargetApp.Models
{
    public class TargetApp
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
        public int LastStatus { get; set; } = 0;
    }
}

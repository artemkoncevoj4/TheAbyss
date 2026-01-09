using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GameServer.Database;

[Table("users")] // Название таблицы в БД
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Login { get; set; } = default!;

    [Required]
    public string PasswordHash { get; set; } = default!;

    // Новые поля для координат
    public float LastPosX { get; set; } = 0f;
    public float LastPosY { get; set; } = 1f;
    public float LastPosZ { get; set; } = 0f;
}
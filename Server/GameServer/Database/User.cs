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
}
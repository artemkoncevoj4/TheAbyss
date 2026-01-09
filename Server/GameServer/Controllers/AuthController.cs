using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameServer.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IPasswordHasher _hasher;

    public AuthController(AppDbContext db, IPasswordHasher hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest req)
    {
        if (await _db.User.AnyAsync(u => u.Login == req.Login))
            return BadRequest("Логин занят");

        var user = new User {
            Login = req.Login,
            PasswordHash = _hasher.Hash(req.Password)
        };

        _db.User.Add(user);
        await _db.SaveChangesAsync();
        return Ok("Регистрация успешна");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest req)
    {
        var user = await _db.User.FirstOrDefaultAsync(u => u.Login == req.Login);
        if (user == null || !_hasher.Verify(req.Password, user.PasswordHash))
            return Unauthorized("Неверный логин или пароль");

        // Возвращаем ID и сохраненные координаты
        return Ok(new { 
            userId = user.Id, 
            login = user.Login,
            posX = user.LastPosX, 
            posY = user.LastPosY, 
            posZ = user.LastPosZ 
        });
    }
}

public record AuthRequest(string Login, string Password);
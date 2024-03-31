using Microsoft.EntityFrameworkCore;

namespace MyService.BD;

public class BDManage : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    
    public BDManage(DbContextOptions<BDManage> options) : base(options)
    {
        Database.EnsureCreated(); 
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, UserName = "kesha101", FirstName = "Kesha", LastName = "Ivanov", Email = "kesha2024@mail.ru", Phone = "+79865643425"},
            new User { Id = 2, UserName = "vesta69", FirstName = "Vesta", LastName = "Sergeeva", Email = "vesta69@mail.ru", Phone = "+79765643679" },
            new User { Id = 3, UserName = "smurf999", FirstName = "Maksim", LastName = "Petrov", Email = "maksim89@mail.ru", Phone = "+79087654536" }
        );
    }

    public IResult GetUser(long id)
    {
        User? user = Users.FirstOrDefault(u => u.Id == id);
        // если не найден, отправляем статусный код и сообщение об ошибке
        if (user == null)  return Results.NotFound(new { message = "Пользователь не найден" });
 
        // если пользователь найден, отправляем его
        return Results.Json(user);
    }
    
    public async Task<IResult> DeleteUser(long id)
    {
        // получаем пользователя по id
        User? user = Users.FirstOrDefault(u => u.Id == id);
 
        // если не найден, отправляем статусный код и сообщение об ошибке
        if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
 
        // если пользователь найден, удаляем его
        Users.Remove(user);
        await SaveChangesAsync();
        return Results.Json(user);
    }

    public async Task<IResult> AddUser(AddUser user)
    {
        var id = Convert.ToInt64(Users.Count() + 1);
        var newUser = User.CreateUser(id, user);
        try
        {
            await Users.AddAsync(newUser);
            await SaveChangesAsync();
            return Results.Json("Пользователь добавлен с id=" + id);
        }
        catch (Exception e)
        {
            return Results.Problem(e.ToString());
        }
    }

    public async Task<IResult> UpdateUser(User user)
    {
        // получаем пользователя по id
        var oldUser = Users.FirstOrDefault(u => u.Id == user.Id);
        // если не найден, отправляем статусный код и сообщение об ошибке
        if (user == null) return Results.NotFound(new { message = "Пользователь не найден" });
        // если пользователь найден, изменяем его данные и отправляем обратно клиенту

        User.UpdateUser(user, oldUser);
        await SaveChangesAsync();
        return Results.Json(oldUser);
    }
}
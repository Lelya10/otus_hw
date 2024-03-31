namespace MyService;

public class User
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public static User CreateUser(long id, AddUser newUser)
    {
        return new User
        {
            Id = id,
            UserName = newUser.UserName,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            Phone = newUser.Phone
        };
    }
    
    
    public static  void UpdateUser(User newUser, User oldUser)
    {
        oldUser.UserName = newUser.UserName;
        oldUser.FirstName = newUser.FirstName;
        oldUser.LastName = newUser.LastName;
        oldUser.Email = newUser.Email;
        oldUser.Phone = newUser.Phone;
    }
}
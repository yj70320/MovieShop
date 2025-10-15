namespace MovieShopMVC.Services
{
    public interface ICurrentUser
    {
        // 只有 getter 没有 setter，因为这个类只提供只读信息给 html，不希望进行任何修改
        // 没有写访问修饰符，就默认是 private
        int? UserId { get; }
        bool IsAdmin { get; }
        bool IsAuthenticated { get; }
        string Email { get; }
        string FullName { get; }
        IEnumerable<string> Roles { get; }
    }
}

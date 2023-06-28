using BlazorApp1.Features.Users;

namespace BlazorApp1;

public static class Mappings
{
    public static UserDto ToDto(this User user) => new (user.Id, user.Name);

    public static UserInfoDto ToDto2(this User user) => new (user.Id, user.Name);
}

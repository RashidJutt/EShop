namespace Basket.Core.Entities;

public class UserInfo
{
    public string UserName { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public AddressInfo Address { get; set; } = new AddressInfo();
}
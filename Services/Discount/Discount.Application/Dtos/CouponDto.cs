using ProtoBuf;

namespace Discount.Application.Dtos;

[ProtoContract]
[CompatibilityLevel(CompatibilityLevel.Level300)]
public class CouponDto 
{
    [ProtoMember(1)]
    public int Id { get; set; }
    [ProtoMember(2)]
    public string ProductName { get; set; } = default!;

    [ProtoMember(3)]
    public string Description { get; set; } = default!;

    [ProtoMember(4)]
    public decimal Amount { get; set; }
}

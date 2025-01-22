using ProtoBuf;

namespace Discount.Application.Dtos;

[ProtoContract]
[CompatibilityLevel(CompatibilityLevel.Level300)]
public class CouponRequestDto
{
    [ProtoMember(1)]
    public string ProductName { get; set; } = default!;
}

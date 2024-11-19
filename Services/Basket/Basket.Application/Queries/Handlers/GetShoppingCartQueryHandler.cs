using AutoMapper;
using Basket.Application.Dtos;
using Basket.Application.Exceptions;
using Basket.Application.Validators;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Queries.Handlers;

public class GetShoppingCartQueryHandler : IRequestHandler<GetShoppingCartQuery, ShoppingCartDto>
{
    private readonly IBasketRepository _basketRepository;
    private readonly IEnumerable<IShoppingCartValidator> _validators;
    private readonly IMapper _mapper;

    public GetShoppingCartQueryHandler(
        IBasketRepository basketRepository,
        IEnumerable<IShoppingCartValidator> validators,
        IMapper mapper)
    {
        _basketRepository = basketRepository;
        _validators = validators;
        _mapper = mapper;
    }
    public async Task<ShoppingCartDto> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        var shoppingCart = await _basketRepository.GetAsync(request.UserName, cancellationToken);

        foreach (var validator in _validators)
        {
            validator.Validate(shoppingCart, request.UserName);
        }

        return _mapper.Map<ShoppingCartDto>(shoppingCart)!;
    }
}

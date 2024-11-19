using AutoMapper;
using Catalog.Application.Dtos;
using Catalog.Application.Queries;
using Catalog.Core.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Handlers
{
    public class GetAllTypesHandler : IRequestHandler<GetAllTypesQuery, IList<TypeDto>>
    {
        private readonly ITypeRepository _typesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTypesHandler> _logger;

        public GetAllTypesHandler(ITypeRepository typesRepository, IMapper mapper, ILogger<GetAllTypesHandler> logger)
        {
            _typesRepository = typesRepository ?? throw new ArgumentNullException(nameof(typesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IList<TypeDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typesRepository.GetAllAsync(cancellationToken);

            if (types == null || !types.Any())
            {
                _logger.LogWarning("No product types found.");
                return new List<TypeDto>();
            }

            var typeDtos = _mapper.Map<IList<TypeDto>>(types);
            return typeDtos;
        }
    }
}

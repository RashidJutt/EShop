using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Dtos
{
    public class BrandDto : EntityDto
    {
        public string Name { get; set; } = default!;
    }
}

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Worker_GrpcService.DAL;
using Worker_GrpcService.Protos.Server;

namespace Worker_GrpcService.Services
{
    public class GenderService: GenderGrpcService.GenderGrpcServiceBase
    {
        private readonly ApplicationDBContext _dbContext;

        public GenderService(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<GetAllGendersResponse> GetAllGenders(Empty request, ServerCallContext context)
        {
            var response = new GetAllGendersResponse();

            var genders = await _dbContext.Genders.Select(g => new GenderGrpc
            {
                Id = g.Id,
                Name = g.Name

            }).ToListAsync();

            response.Genders.AddRange(genders);

            return await Task.FromResult(response);
        }
    }
}

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Server.IIS.Core;
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

        public override async Task<GenderGrpc> GetGenderById(GetGenderByIdRequest request, ServerCallContext context)
        {
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Id не может быть меньше 1"));

            var gender = await _dbContext.Genders.FirstOrDefaultAsync(g => g.Id == request.Id);

            if (gender != null)
            {
                return await Task.FromResult(new GenderGrpc
                {
                    Id = gender.Id,
                    Name = gender.Name
                });
            }

            throw new RpcException(new Status(StatusCode.NotFound, "Данный gender не найден"));
        }
    }
}

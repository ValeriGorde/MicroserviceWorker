using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Worker_GrpcService.DAL;
using Worker_GrpcService.Protos.Server;

namespace Worker_GrpcService.Services
{
    public class WorkerService: WorkerGrpcService.WorkerGrpcServiceBase
    {
        private readonly ApplicationDBContext _dbContext;

        public WorkerService(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public override async Task<GetAllWorkersResponse> GetAllWorkers(Empty request, ServerCallContext context)
        {
            var response = new GetAllWorkersResponse();

            var workers = await _dbContext.Workers.Select(w => new WorkerResponse
            {
                Id = w.Id,
                FirstName = w.FirstName,
                LastName = w.LastName,
                Patronymic = w.Patronymic,
                BirthDate = w.BirthDate,
                GenderId = w.GenderId,
                HasChildren = w.HasChildren
            }).ToListAsync();

            response.Workers.AddRange(workers);

            return await Task.FromResult(response);
        }
    }
}

using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Worker_GrpcService.DAL;
using Worker_GrpcService.DAL.Models;
using Worker_GrpcService.Protos.Server;

namespace Worker_GrpcService.Services
{
    public class WorkerService : WorkerGrpcService.WorkerGrpcServiceBase
    {
        private readonly ApplicationDBContext _dbContext;

        public WorkerService(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public override async Task<WorkerResponse> CreateWorker(CreateWorkerRequest request, ServerCallContext context)
        {
            if (request.FirstName == string.Empty || request.LastName == string.Empty ||
                request.Patronymic == string.Empty || request.BirthDate == string.Empty ||
                request.Gender == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Неккоректные исходные данные"));

            var worker = new Worker
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic,
                BirthDate = request.BirthDate,
                Gender = request.Gender,
                HasChildren = request.HasChildren
            };

            await _dbContext.Workers.AddAsync(worker);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(new WorkerResponse
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                Gender = worker.Gender,
                BirthDate = worker.BirthDate,
                HasChildren = worker.HasChildren
            });
        }

        public override async Task<WorkerResponse> DeleteWorker(DeleteWorkerRequest request, ServerCallContext context)
        {
            if (request.Id <= 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Неккоректные исходные данные"));

            var worker = await _dbContext.Workers.FirstOrDefaultAsync(w => w.Id == request.Id);
            if (worker == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Работник не найден"));

            _dbContext.Workers.Remove(worker);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(new WorkerResponse
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                BirthDate = worker.BirthDate,
                Gender = worker.Gender,
                HasChildren = worker.HasChildren
            });
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
                Gender = w.Gender,
                HasChildren = w.HasChildren
            }).ToListAsync();

            response.Workers.AddRange(workers.ToArray());

            return await Task.FromResult(response);
        }

        public override async Task<WorkerResponse> UpdateWorker(UpdateWorkerRequest request, ServerCallContext context)
        {
            if (request.Id <= 0 || request.FirstName == string.Empty || request.LastName == string.Empty ||
                request.Patronymic == string.Empty || request.BirthDate == string.Empty ||
                request.Gender == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Неккоректные данные"));

            var worker = _dbContext.Workers.FirstOrDefault(w => w.Id == request.Id);

            if(worker != null)
            {
                worker.Id = request.Id;
                worker.FirstName = request.FirstName;
                worker.LastName = request.LastName;
                worker.BirthDate = request.BirthDate;
                worker.Gender = request.Gender;
                worker.HasChildren = request.HasChildren;

                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(new WorkerResponse
                {
                    Id = worker.Id,
                    FirstName = worker.FirstName,
                    LastName = worker.LastName,
                    Patronymic = worker.Patronymic,
                    BirthDate = worker.BirthDate,
                    Gender = worker.Gender,
                    HasChildren = worker.HasChildren
                });
            }

            throw new RpcException(new Status(StatusCode.NotFound, "Пользователь не найден"));
        }
    }
}

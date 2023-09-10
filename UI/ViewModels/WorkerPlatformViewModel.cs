using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Protos.Client;
using Worker_GrpcService.DAL.Models;

namespace UI.ViewModels
{
    public class WorkerPlatformViewModel: BaseViewModel
    {        
        private readonly GenderGrpcService.GenderGrpcServiceClient genderGrpcClient;
        private readonly WorkerGrpcService.WorkerGrpcServiceClient workerGrpcService;

        public WorkerPlatformViewModel()
        {
            // Устанавливам обработчик HTTP запросов с проверкой сертификатов для безопасной связт между клиентом и сервером
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channelOptions = new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            };
            var channel = GrpcChannel.ForAddress("https://localhost:7139", channelOptions);
            genderGrpcClient = new GenderGrpcService.GenderGrpcServiceClient(channel);
            workerGrpcService = new WorkerGrpcService.WorkerGrpcServiceClient(channel);

            OnCreating();
        }

        public void OnCreating() 
        {
            Task.Run(() => GetAllGenders());
            Task.Run(() => GetAllWorkers());
        }


        #region Worker
        private List<Worker> workers;
        public List<Worker> Workers
        {
            get { return workers; }
            set
            {
                workers = value;
                OnPropertyChanged();
            }
        }

        private Worker selectedWorker;
        public Worker SelectedWorker
        {
            get { return selectedWorker; }
            set
            {
                selectedWorker = value;
                OnPropertyChanged();

                if (selectedWorker != null)
                {
                    FirstName = selectedWorker.FirstName;
                    LastName = selectedWorker.LastName;
                    Patronymic = selectedWorker.Patronymic;
                    BirthDate = selectedWorker.BirthDate;
                    Gender = selectedWorker.Gender;
                    HasChildren = selectedWorker.HasChildren;
                }
            }
        }

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        private string patronymic;
        public string Patronymic
        {
            get { return patronymic; }
            set
            {
                patronymic = value;
                OnPropertyChanged();
            }
        }

        private string birthDate;
        public string BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged();
            }
        }

        private bool hasChildren;
        public bool HasChildren
        {
            get { return hasChildren; }
            set
            {
                hasChildren = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Gender
        private List<Gender> genders;
        public List<Gender> Genders
        {
            get { return genders; }
            set
            {
                genders = value;
                OnPropertyChanged();
            }
        }

        private Gender gender;
        public Gender Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Метод для получения всех работников
        /// </summary>
        /// <returns></returns>
        public async Task GetAllWorkers()
        {
            var response = await workerGrpcService.GetAllWorkersAsync(new Empty());

            var workers = response.Workers.Select(w => new Worker
            {
                Id = w.Id,
                FirstName = w.FirstName,
                LastName = w.LastName,
                Patronymic = w.Patronymic,
                BirthDate = w.BirthDate,
                GenderId = w.GenderId,
                HasChildren = w.HasChildren
            }).ToList();

            Workers = new List<Worker>(workers);
        }


        /// <summary>
        /// Метод для получения всех гендеров)
        /// </summary>
        /// <returns></returns>
        public async Task GetAllGenders()
        {
            var request = new Empty();
            var response = await genderGrpcClient.GetAllGendersAsync(request);

            var genders = response.Genders.Select(g => new Gender
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();

            Genders = new List<Gender>(genders);
        }















    }
}

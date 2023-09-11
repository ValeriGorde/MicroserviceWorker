using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UI.Commands;
using UI.Protos.Client;
using Worker_GrpcService.DAL.Models;

namespace UI.ViewModels
{
    public class WorkerPlatformViewModel : BaseViewModel
    {
        private readonly GenderGrpcService.GenderGrpcServiceClient genderGrpcClient;
        private readonly WorkerGrpcService.WorkerGrpcServiceClient workerGrpcClient;

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
            workerGrpcClient = new WorkerGrpcService.WorkerGrpcServiceClient(channel);

            OnCreating();
        }

        public void OnCreating()
        {
            Task.Run(() => GetAllGenders());
            Task.Run(() => GetAllWorkers());
        }


        #region Worker
        private ObservableCollection<Worker> workers;
        public ObservableCollection<Worker> Workers
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
                    GenderId = selectedWorker.GenderId;
                    HasChildren = selectedWorker.HasChildren;
                    Gender = selectedWorker.Gender;
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
        private ObservableCollection<Gender> genders;
        public ObservableCollection<Gender> Genders
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

        private int genderId;
        public int GenderId
        {
            get { return genderId; }
            set
            {
                genderId = value;
                OnPropertyChanged();
            }
        }

        #endregion

        /// <summary>
        /// Команда по созданию работника
        /// </summary>
        private RelayCommand createWorkerCommand;
        public RelayCommand CreateWorkerCommand
        {
            get
            {
                return createWorkerCommand ??= new RelayCommand(async x =>
                {
                    await CreateWorker(new Worker
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                        Patronymic = Patronymic,
                        BirthDate = BirthDate,
                        GenderId = Gender.Id,
                        HasChildren = HasChildren
                    });
                });
            }
        }

        /// Команда по удалению работника
        /// </summary>
        private RelayCommand deleteWorkerCommand;
        public RelayCommand DeleteWorkerCommand
        {
            get
            {
                return deleteWorkerCommand ??= new RelayCommand(async x =>
                {
                    await DeleteWorker(SelectedWorker);
                });
            }
        }

        public async Task DeleteWorker(Worker worker)
        {
            var request = new DeleteWorkerRequest
            {
                Id = worker.Id
            };

            var response = await workerGrpcClient.DeleteWorkerAsync(request);
            var deletedWorker = new Worker
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Patronymic = response.Patronymic,
                BirthDate = response.BirthDate,
                GenderId = response.GenderId,
                HasChildren = response.HasChildren
            };
            Workers.Remove(deletedWorker);
        }

        /// <summary>
        /// Метод для добавления нового работника
        /// </summary>
        /// <returns></returns>
        public async Task CreateWorker(Worker worker)
        {
            var request = new CreateWorkerRequest
            {
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                BirthDate = worker.BirthDate,
                GenderId = worker.GenderId,
                HasChildren = worker.HasChildren
            };

            var response = await workerGrpcClient.CreateWorkerAsync(request);

            var newWorker = new Worker
            {
                Id = response.Id,
                FirstName = response.FirstName,
                LastName = response.LastName,
                Patronymic = response.Patronymic,
                BirthDate = response.BirthDate,
                GenderId = response.GenderId,
                HasChildren = response.HasChildren
            };

            newWorker.Gender = GetGenderById(newWorker.GenderId).Result;
            Workers.Add(newWorker);
        }

        /// <summary>
        /// Метод для получения всех работников
        /// </summary>
        /// <returns></returns>
        public async Task GetAllWorkers()
        {
            var response = await workerGrpcClient.GetAllWorkersAsync(new Empty());

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

            foreach (var worker in workers)
            {
                worker.Gender = GetGenderById(worker.GenderId).Result;
            }

            Workers = new ObservableCollection<Worker>(workers);
        }

        /// <summary>
        /// Метод для гендера по id
        /// </summary>
        /// <returns></returns>
        public async Task<Gender> GetGenderById(int id)
        {
            var request = new GetGenderByIdRequest { Id = id };
            var response = await genderGrpcClient.GetGenderByIdAsync(request);

            if (response != null)
            {
                return new Gender
                {
                    Id = response.Id,
                    Name = response.Name
                };
            }

            return null;
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

            Genders = new ObservableCollection<Gender>(genders);
        }

    }
}

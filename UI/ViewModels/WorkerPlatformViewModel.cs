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
            workerGrpcClient = new WorkerGrpcService.WorkerGrpcServiceClient(channel);

            OnCreating();
        }

        public void OnCreating()
        {
            Genders = new ObservableCollection<string>
            {
                "Женский",
                "Мужской"
            };

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
        private ObservableCollection<string> genders;
        public ObservableCollection<string> Genders
        {
            get { return genders; }
            set
            {
                genders = value;
                OnPropertyChanged();
            }
        }

        private string gender;
        public string Gender
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
                        Gender = Gender,
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

        /// <summary>
        /// Команда по обновлению работника
        /// </summary>
        private RelayCommand updateWorkerCommand;
        public RelayCommand UpdateWorkerCommand
        {
            get
            {
                return updateWorkerCommand ??= new RelayCommand(async x =>
                {
                    await UpdateWorker(new Worker
                    {
                        Id = SelectedWorker.Id,
                        FirstName = FirstName,
                        LastName = LastName,
                        Patronymic = Patronymic,
                        BirthDate = BirthDate,
                        Gender = Gender,
                        HasChildren = HasChildren
                    });
                });
            }
        }

        /// <summary>
        /// Метод по удалению работника
        /// </summary>
        /// <param name="worker"></param>
        /// <returns></returns>
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
                Gender = response.Gender,
                HasChildren = response.HasChildren
            };
            Workers.Remove(deletedWorker);
            OnCreating();
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
                Gender = worker.Gender,
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
                Gender = response.Gender,
                HasChildren = response.HasChildren
            };

            Workers.Add(newWorker);
            OnCreating();
        }

        /// <summary>
        /// Метод для обновления работника
        /// </summary>
        /// <returns></returns>
        public async Task UpdateWorker(Worker worker)
        {
            var request = new UpdateWorkerRequest
            {
                Id = worker.Id,
                FirstName = worker.FirstName,
                LastName = worker.LastName,
                Patronymic = worker.Patronymic,
                BirthDate = worker.BirthDate,
                Gender = worker.Gender,
                HasChildren = worker.HasChildren
            };

            var response = await workerGrpcClient.UpdateWorkerAsync(request);

            var workerNew = Workers.FirstOrDefault(w => w.Id == response.Id);
            if (workerNew != null)
            {
                workerNew.Id = response.Id;
                workerNew.FirstName = response.FirstName;
                worker.LastName = response.LastName;
                worker.BirthDate = response.BirthDate;
                worker.Gender = response.Gender;
                worker.HasChildren = response.HasChildren;
            }

            OnCreating();
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
                Gender = w.Gender,
                HasChildren = w.HasChildren
            }).ToList();

            Workers = new ObservableCollection<Worker>(workers);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace FriendOrganizer.UI.ViewModel
{
    class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _friendRepository;
        private IEventAggregator _eventAggrigator;
        private FriendWrapper _friend;
        private bool _hasChanges;
        private IMessageDialogService _messageDialogService;
        private IProgrammingLanguageLookupDataService _programmingLanguageLookupDataService;

        public FriendDetailViewModel(IFriendRepository friendRepository, IEventAggregator eventAggregator, IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            _friendRepository = friendRepository;
            _eventAggrigator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageLookupDataService = programmingLanguageLookupDataService;


            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }


        public async Task LoadAsync(int? friendId)
        {
            await InitializeFriend(friendId);
            await LoadProgrammingLanguagesLookupAsync();
        }

        private async Task InitializeFriend(int? friendId)
        {
            var friend = friendId.HasValue ? await _friendRepository.GetByIdAsync(friendId.Value) : CreateNewFriend();

            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _friendRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                //Little trick to trigger the validation
                Friend.FirstName = "";
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem());
            var lookup = await _programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        public FriendWrapper Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public ICommand SaveCommand { get;  }
        public ICommand DeleteCommand { get; }

        private async void OnSaveExecute()
        {
            await _friendRepository.SaveAsync();
            HasChanges = _friendRepository.HasChanges();
            _eventAggrigator.GetEvent<AfterFriendSaveEvent>().Publish(new AfterFriendSavedEventArgs()
            {
                Id = Friend.Id,
                DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
            });
        }

        

        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }               
            }
        }


        private bool OnSaveCanExecute()
        {
            return Friend!= null && !Friend.HasErrors && HasChanges;
        }

        private async void OnDeleteExecute()
        {
            var result =
                _messageDialogService.ShowOkCancelDialog(
                    $"Do you really want to delete the friend {Friend.FirstName} {Friend.LastName}", "Question");
            if (result == MessageDialogResult.Ok)
            {
                _friendRepository.Remove(Friend.Model);
                await _friendRepository.SaveAsync();
                _eventAggrigator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }           
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _friendRepository.Add(friend);
            return friend;
        }

    }
}

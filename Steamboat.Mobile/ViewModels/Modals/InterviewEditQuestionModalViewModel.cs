using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Steamboat.Mobile.ViewModels.Modals
{
    public class InterviewEditQuestionModalViewModel: ModalViewModelBase
    {
        #region Properties

        private Func<object, Task> AfterCloseModal;

        public ICommand EditQuestionCommand { get; set; }

        #endregion

        public InterviewEditQuestionModalViewModel()
        {
            IsLoading = true;

            EditQuestionCommand = new Command(async () => await EditQuestion());
        }

        public override Task InitializeAsync(object parameter)
        {
            AfterCloseModal = parameter as Func<object, Task>;
            IsLoading = false;
            return base.InitializeAsync(parameter);
        }

        private async Task EditQuestion()
        {
            try
            {
                IsBusy = true;
                await CloseModal();
                await AfterCloseModal(null);
            }
            catch (Exception e)
            {
                await DialogService.ShowAlertAsync(e.Message, "Error", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

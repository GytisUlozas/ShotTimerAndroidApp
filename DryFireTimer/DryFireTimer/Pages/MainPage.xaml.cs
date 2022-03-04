using DryFireTimer.DataAccess;
using DryFireTimer.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DryFireTimer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly IMyRepository repo;
        private readonly Timer ParTimer = new Timer();
        private readonly Timer BreakTimer = new Timer();
        private Timer RunningTimer;
        private readonly Color myGreen = Color.GreenYellow;
        private readonly Color myYellow = Color.Yellow;

        private int RemainingReps;
        public Exercise Exercise { get; set; }
        public IMyAudioPlayer Player { get; set; }


        public MainPage(IMyRepository _repo)
        {
            InitializeComponent();
            repo = _repo;
            Exercise = repo.GetFirst();

            //Prevent screen from locking
            DeviceDisplay.KeepScreenOn = true;

            //Loads sound player
            Player = DependencyService.Get<IMyAudioPlayer>();
            Player.Load(App.AudioName);
            
            //Wires timer Events
            BreakTimer.OnCountEnd += ()=>
            {
                RunningTimer.Reset();
                RunningTimer = ParTimer;
                RunningTimer.Start();
            };

            ParTimer.OnIncrement += UpdateClock;
            ParTimer.OnCountStart += () => Player.Play();
            ParTimer.OnCountEnd += () =>
            {
                Player.Play();

                RemainingReps--;
                SetRepsLabel();

                RunningTimer.Reset();
                RunningTimer = BreakTimer;

                if (RemainingReps > 0)
                    RunningTimer.Start();
                else
                {
                   UpdatePage();
                }
            };

            //Draws Interface and sets the timers
            UpdatePage();
        }

        public void OnStartButtonClick(object sender, EventArgs args)
        {
            if (RunningTimer.IsRunning)
            {
                FillStartButton();
                RunningTimer.Pause();
            }
            else
            {
                StartButton.Text = "Pause";
                StartButton.BackgroundColor = myYellow;
                RunningTimer.Start();
            }
        }

        public void OnResetButtonClick(object sender, EventArgs args)
        {
            ParTimer.Pause();
            BreakTimer.Pause();
            UpdatePage();
        }

        public void OnEditButtonClick(object sender, EventArgs args)
            => Navigation.PushModalAsync(new EditPage(this, Exercise, false, repo));

        public void OnPrevButtonClick(object sender, EventArgs args)
        {
            Exercise newEx = repo.GetPrevious(Exercise);
            if (newEx == null)
                DisplayAlert("Nowhere to go", "This is the first exercise", "OK");
            else
            {
                Exercise = newEx;
                UpdatePage();
            }
        }

        public async void OnNextButtonClick(object sender, EventArgs args)
        {
            Exercise newEx = repo.GetNext(Exercise);
            if (newEx == null)
            {
                bool createNew = await DisplayAlert("Nowhere to go", "That was the last exercise. Create new?", "OK", "Cancel");
                if (createNew)
                    await Navigation.PushModalAsync(new EditPage(this, Exercise.NewlyCreated, true, repo));
            }
            else
            {
                Exercise = newEx;
                UpdatePage();
            }
        }

        private void UpdateClock (int time) 
           => Device.BeginInvokeOnMainThread(()=>UiClock.Text = $"{time / 600}:{(time < 100 ? "0" : "")}{time / 10}.{time % 10}");
        
        public void UpdatePage()
        {
            
            //Resets rep count
            RemainingReps = Exercise.NumberOfReps;

            //Sets timers
            RunningTimer = BreakTimer;
            BreakTimer.StopCount = Exercise.BreakTime;
            ParTimer.StopCount = Exercise.ParTime;
            BreakTimer.Reset();
            ParTimer.Reset();

            Device.BeginInvokeOnMainThread(() =>
            {
                //Fills interface
                ExerciseNameLabel.Text = Exercise.Name;
                ExerciseTimeLabel.Text = $"Par time: {Timer.ToSeconds(Exercise.ParTime)} s";
                BreakTimeLabel.Text = $"Wait time: {Timer.ToSeconds(Exercise.BreakTime)} s";
                TotalTimeLabel.Text = $"Total exercise span: {Timer.Min(Exercise.TotalTime)} m {Timer.Sec(Exercise.TotalTime)} s";
                DescriptionLabel.Text = Exercise.Description;
                FillStartButton();
                SetRepsLabel();
            });
        }

        private void FillStartButton()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                StartButton.Text = "Start";
                StartButton.BackgroundColor = myGreen;
            });
        }
        private void SetRepsLabel()
            => Device.BeginInvokeOnMainThread(() => { NumberOfRepsLabel.Text = $"Repetitions left: {RemainingReps} of {Exercise.NumberOfReps}"; });
        
        protected override bool OnBackButtonPressed()
        {
            OnPrevButtonClick(new object(), new EventArgs());
            return true;
        }
    }
}

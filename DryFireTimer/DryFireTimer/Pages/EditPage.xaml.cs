using DryFireTimer.DataAccess;
using DryFireTimer.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DryFireTimer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditPage : ContentPage
    {
        private readonly Exercise exercise;
        private readonly IMyRepository repo;
        private readonly bool isNew;
        private readonly MainPage mainPage;
        public EditPage(MainPage _sender, Exercise _ex, bool _isNew, IMyRepository _repo)
        {
            InitializeComponent();
            exercise = _ex;
            isNew = _isNew;
            repo = _repo;
            mainPage = _sender;

            UpdateUI();
        }

        public void OnSaveButtonClick(object sender, EventArgs args)
        {
            bool failure = false;

            if (Timer.ToTime(ExerciseTimeEntry.Text, out int a))
            { }
            else
            {
                ExerciseTimeEntry.TextColor = Color.Red;
                failure = true;
            }

            if (Timer.ToTime(BreakTimeEntry.Text, out int b))
            { }
            else
            {
                BreakTimeEntry.TextColor = Color.Red;
                failure = true;
            }

            if (Timer.ToReps(NumberOfRepsEntry.Text, out int c))
            { }
            else
            {
                NumberOfRepsEntry.TextColor = Color.Red;
                failure = true;
            }

            if(failure)
                DisplayAlert("Invalid input", "Please edit the fields in red", "OK");
            else
            {
                exercise.ParTime = a;
                exercise.BreakTime = b;
                exercise.NumberOfReps = c;
                exercise.Name = ExerciseNameEntry.Text;
                exercise.Description = DescriptionEntry.Text;

                if (isNew)
                    repo.Create(exercise);
                else
                    repo.Update(exercise);

                mainPage.Exercise = exercise;
                mainPage.UpdatePage();

                OnReturnButtonClick(new object(), new EventArgs());
            }
        }

        public void OnReturnButtonClick(object sender, EventArgs args)
            => Navigation.PopModalAsync();
        
        public void OnDeleteButtonClick(object sender, EventArgs args)
        {
                repo.Delete(exercise);
                mainPage.Exercise = repo.GetPrevious(exercise);
                mainPage.UpdatePage();
                OnReturnButtonClick(new object(), new EventArgs());
        }

        private void UpdateUI()
        {
            ExerciseNameLabel.Text = "Exercise name";
            ExerciseNameEntry.Text = exercise.Name;
            
            ExerciseTimeLabel.Text = "Par time";
            ExerciseTimeEntry.Text = Timer.ToSeconds(exercise.ParTime);

            BreakTimeLabel.Text = "Wait time";
            BreakTimeEntry.Text = Timer.ToSeconds(exercise.BreakTime);

            NumberOfRepsLabel.Text = "Number of repetitions";
            NumberOfRepsEntry.Text = (exercise.NumberOfReps).ToString();

            DescriptionLabel.Text = "Exercise description";
            DescriptionEntry.Text = exercise.Description;

            if(isNew || repo.GetFirst().Id == exercise.Id)
                DeleteButton.IsEnabled = false;
        }
        protected override bool OnBackButtonPressed()
        {
            OnReturnButtonClick(new object(), new EventArgs());
            return true; 
        }
    }
}
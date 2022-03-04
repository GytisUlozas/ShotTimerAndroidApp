using SQLite;

namespace DryFireTimer.Models
{
    [Table("Exercises")]
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParTime { get; set; } //Tenths of a second not a full one
        public int NumberOfReps { get; set; }
        public int BreakTime { get; set; } //Tenths of a second not a full one

        [Ignore]
        public int TotalTime => (ParTime + BreakTime) * NumberOfReps;

        [Ignore]
        public static Exercise Default => new Exercise()
            {
                Id = 1,
                Name = "Quick draw",
                Description = "Draw your weapon from the holster, aim and fire",
                ParTime = 15,
                NumberOfReps = 5,
                BreakTime = 50
            };
        [Ignore]
        public static Exercise NewlyCreated => new Exercise()
        {
            Name = "Exercise name here",
            Description = "Exercise description here",
            ParTime = 15,
            NumberOfReps = 5,
            BreakTime = 50
        };

    }
}

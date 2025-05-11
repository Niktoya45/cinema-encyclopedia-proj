

namespace UserDataService.Domain.Aggregates.UserAggregate
{
    public record LabeledRecord:UserCinemaRecord
    {
        public Label Label { get; set; } = Label.None;
    }
}

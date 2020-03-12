namespace Conference.SpeakerProfiles
{
    public class SpeakerProfileModel : EntityModel
    {
        public string TagLine { get; set; } = null!;

        public string Bio { get; set; } = null!;

        public UserModel User { get; set; } = null!;
    }
}
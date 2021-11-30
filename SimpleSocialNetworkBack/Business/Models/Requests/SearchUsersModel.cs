namespace Business.Models.Requests
{
    public class SearchUsersModel
    {
        public string? NamePattern { get; set; }

        public string? AboutPattern { get; set; }
    }
}

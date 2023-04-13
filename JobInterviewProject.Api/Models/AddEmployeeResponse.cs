namespace JobInterviewProject.Api.Models
{
    public class AddEmployeeResponse
    {
        public string PersonalId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}

namespace JobInterviewProject.Domain.Exceptions;

public class PersonalIdAlreadyExistsException : Exception
{
    public PersonalIdAlreadyExistsException(string message) : base(message) { } 
}

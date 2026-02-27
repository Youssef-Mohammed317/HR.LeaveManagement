namespace HR.LeaveManagement.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string typeName, object key) : base($"{typeName} ({key}) was not found")
    {

    }
    public NotFoundException(string message) : base(message)
    {

    }
}

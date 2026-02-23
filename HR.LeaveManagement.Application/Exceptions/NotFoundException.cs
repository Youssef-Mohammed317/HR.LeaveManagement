namespace HR.LeaveManagement.Application.Exceptions;

public class NotFoundException(string typeName, object key) : Exception($"{typeName} ({key}) was not found");


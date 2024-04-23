using System.Net;

namespace MobyLabWebProgramming.Core.Errors;

/// <summary>
/// Common error messages that may be reused in various places in the code.
/// </summary>
public static class CommonErrors
{
    public static ErrorMessage UserNotFound => new(HttpStatusCode.NotFound, "User doesn't exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage FileNotFound => new(HttpStatusCode.NotFound, "File not found on disk!", ErrorCodes.PhysicalFileNotFound);
    public static ErrorMessage TechnicalSupport => new(HttpStatusCode.InternalServerError, "An unknown error occurred, contact the technical support!", ErrorCodes.TechnicalError);
    public static ErrorMessage ProfileNotFound => new(HttpStatusCode.NotFound, "Profile is not created!", ErrorCodes.EntityNotFound);
    public static ErrorMessage ActivitySessionNotFound => new(HttpStatusCode.NotFound, "ActivitySession does not exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage ActivityTypeNotFound => new(HttpStatusCode.NotFound, "Activity Type does not exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage GoalNotFound => new(HttpStatusCode.NotFound, "Goal has not been set!", ErrorCodes.EntityNotFound);
    public static ErrorMessage WorkoutProgramNotFound => new(HttpStatusCode.NotFound, "Workout Program does not exist!", ErrorCodes.EntityNotFound);
    public static ErrorMessage SubscriptionNotFound => new(HttpStatusCode.NotFound, "User is not subscribed to this workout program!", ErrorCodes.EntityNotFound);
}

using System;
using System.Linq;
using Business.Validation;
using DataAccess.Entities;

namespace Business.Common
{
    internal static class ExceptionHelper
    {
        public static void CheckEntitySoft<T>(T? entity, string userArgName) where T : class, ISoftDelete
        {
            if (entity == null)
                throw new ValidationException($"Nonexistent {userArgName}");
            if (entity.IsDeleted)
                throw new ValidationException($"{userArgName} was deleted");
        }

        public static void CheckSelfSoft<T>(T? entity, string userArgName) where T : class, ISoftDelete
        {
            if (entity == null)
                throw new BadCredentialsException($"Nonexistent {userArgName}");
            if (entity.IsDeleted)
                throw new ValidationException($"Current {userArgName} was deleted");
        }

        public static void ThrowIfUserBanned(ApplicationUser entity)
        {
            if (entity.BansReceived.Any(ban => !ban.Cancelled && ban.ExpirationDate > DateTime.UtcNow))
                throw new ForbiddenException("You are banned");
        }
    }
}

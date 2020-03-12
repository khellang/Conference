using System;

namespace Conference
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
    }
}
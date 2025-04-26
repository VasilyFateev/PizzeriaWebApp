namespace AdminApp.LocalExceptions
{
    public class NonUniqueNameException<T>() : Exception()
    {
        public override string Message => $"An attempt to add a <{typeof(T)}Ю type object with a non-unique name to the database.\n" + base.Message;
    }

    public class MissingForeignKeyException<T>() : Exception()
    {
        public override string Message => $"ANo foreign key has been set for an object of type <{typeof(T)}>\n" + base.Message;
    }
}

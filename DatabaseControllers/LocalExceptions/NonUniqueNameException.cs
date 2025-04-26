namespace AdminApp.CustomExceptions
{
    public class NonUniqueNameException(Type T) : Exception()
    {
        public override string Message => $"An attempt to add a <{T}> type object with a non-unique name to the database.\n" + base.Message;
    }

    public class MissingForeignKeyException(Type T) : Exception()
    {
        public override string Message => $"An foreign key has been set for an object of type <{T}>\n" + base.Message;
    }

    public class NotFoundEntityByKeyException(int id, Type T) : Exception()
    {
        public override string Message => $"The <{T}> was not found under this Key-{id}. Check the correctness of the entered ID.\n" + base.Message;
    }
    public class NotFoundEntityByCompositeKeyException(int id1, int id2, Type T) : Exception()
    {
        public override string Message => $"The <{T}> was not found under this Composite key - ({id1}x{id2}). Check the correctness of the entered ID.\n" + base.Message;
    }
}

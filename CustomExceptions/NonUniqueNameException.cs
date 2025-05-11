namespace AssortmentEditService.CustomExceptions
{
    public class NonUniqueNameException(Type T) : Exception()
    {
        public override string Message => $"An attempt to add a <{T}> type object with a non-unique name to the database.\n" + base.Message;
    }
}

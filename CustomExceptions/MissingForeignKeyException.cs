namespace AssortmentEditService.CustomExceptions
{
    public class MissingForeignKeyException(Type T) : Exception()
    {
        public override string Message => $"An foreign key has been set for an object of type <{T}>\n" + base.Message;
    }
}

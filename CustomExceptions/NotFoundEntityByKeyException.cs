namespace AssortmentEditService.CustomExceptions
{
    public class NotFoundEntityByKeyException(int id, Type T) : Exception()
    {
        public override string Message => $"The <{T}> was not found under this Key-{id}. Check the correctness of the entered ID.\n" + base.Message;
    }
}

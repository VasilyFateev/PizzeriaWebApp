namespace AssortmentEditService.CustomExceptions
{
    public class NotFoundEntityByCompositeKeyException(int id1, int id2, Type T) : Exception()
    {
        public override string Message => $"The <{T}> was not found under this Composite key - ({id1}x{id2}). Check the correctness of the entered ID.\n" + base.Message;
    }
}

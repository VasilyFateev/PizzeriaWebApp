namespace AssortmentEditService.DatabaseControllers
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetObjectsList();
        public Task<T?> Get(int id);
        public Task Add(T item);
        public Task Update(T item);
        public Task Delete(int id);
    }
}
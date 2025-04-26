using Microsoft.EntityFrameworkCore;

namespace AdminApp.DatabaseControllers
{

    public class TableChanges<T> where T : class
    {
        public List<T> ToAdd { get; set; } = [];
        public List<T> ToRemove { get; set; } = [];
        public List<T> ToUpdate { get; set; } = [];
    }
}
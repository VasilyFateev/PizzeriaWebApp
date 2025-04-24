using DatabaseAccess;

using (ApplicationContext db = new())
{
    bool isCreated = db.Database.EnsureCreated();
    if (isCreated) Console.WriteLine("The database has been created");
    else Console.WriteLine("The database already exists");
}
using DatabaseAccess;

using (AssortementSetupApplicationContext db = new())
{
    bool isAvalaible = db.Database.CanConnect();
    if (isAvalaible)
    {
        Console.WriteLine("The database is available");
    }
    else
    {
        Console.WriteLine("The database isn't available");
        return;
    }
}

Console.ReadLine();
using HelloEntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

void CreerEmploye()
{
    Employee e1 = new()
    {
        LastName = "Duck",
        FirstName = "Donald"
    };
    NorthwindContext context = new NorthwindContext();
    context.Add(e1);

    Console.WriteLine("avant l'enregistrement: " + e1.EmployeeId);
    context.SaveChanges();
    Console.WriteLine("après l'enregistrement: " + e1.EmployeeId);
}
//CreerEmploye();

void ModifierEmploye()
{
    NorthwindContext context = new NorthwindContext();
    Employee e1 = context.Employees.Find(10) ?? throw new Exception("pas d'employe n°10");
    e1.City = "Bordeaux";
    context.SaveChanges();
}
//ModifierEmploye();

void SupprimerEmploye()
{
    NorthwindContext context = new NorthwindContext();
    Employee e1 = context.Employees.Find(10) ?? throw new Exception("pas d'employe n°10");
    context.Remove(e1);
    context.SaveChanges();
}
//SupprimerEmploye();

void AugmenterPrixCatalogue()
{
    NorthwindContext context = new NorthwindContext();
    List<Product> products = context.Products.ToList();
    products.ForEach(p => p.UnitPrice *= 1.1m);
    context.SaveChanges();
}
//AugmenterPrixCatalogue();

void AugmenterPrixCatalogueBis()
{
    NorthwindContext context = new();
    context.Database.ExecuteSqlInterpolated($"Update Products Set UnitPrice *= 1.1");
}
//AugmenterPrixCatalogueBis();

void AfficherListingEmployes()
{
    NorthwindContext context = new NorthwindContext();
    var req = context.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
    List<Employee> employees = req.ToList();
    foreach (var e in employees)
    {
        Console.WriteLine($"{e.LastName,-10} {e.FirstName,-10} {e.City,10}");
    }
}
//AfficherListingEmployes();

/// <summary>
/// ne pas afficher et comptabiliser les produits discontinués
/// categorie Product Price
/// Beverages (4)
///   Chai $23.95
///   Truc $23.95
/// </summary>
void AfficherCatalogue()
{
    NorthwindContext context = new NorthwindContext();
    var req = context.Categories.OrderBy(c => c.CategoryName);

    Console.WriteLine(req.ToQueryString());
    List<Category> categories = req.ToList();

    foreach (var c in categories)
    {
        var req2 = context.Products.Where(p => p.CategoryId == c.CategoryId && !p.Discontinued).OrderBy(p => p.ProductName);

        Console.WriteLine(req2.ToQueryString());
        /*c.Products = */
        req2.ToList();
        Console.WriteLine($"{c.CategoryName} ({c.Products.Count})");
        foreach (var p in c.Products)
        {
            Console.WriteLine($"  {p.ProductName} ${p.UnitPrice:N2}");
        }
    }
}
//AfficherCatalogue();


void AfficherCatalogueBis()
{
    NorthwindContext context = new NorthwindContext();
    var req = context.Categories
        .Include(c => c.Products.Where(p => !p.Discontinued))
        .OrderBy(c => c.CategoryName);

    Console.WriteLine(req.ToQueryString());
    List<Category> categories = req.ToList();

    foreach (var c in categories)
    {
        Console.WriteLine($"{c.CategoryName} ({c.Products.Count})");
        int maxLength = c.Products.Max(p => p.ProductName.Length);
        foreach (var p in c.Products)
        {
            string nom = string.Format("{0,-" + maxLength + "}", p.ProductName);
            string prix = "$" + string.Format("{0:N2}", p.UnitPrice);
            Console.WriteLine($"  {nom} {prix,7}");
            //Console.WriteLine($"  {p.ProductName} ${p.UnitPrice:N2}");
        }
    }
}
//AfficherCatalogueBis();

/// <summary>
/// Produit le plus vendu du magasin
/// </summary>
void AfficherStatistiques()
{
    var chrono = Stopwatch.StartNew();
    NorthwindContext ctx = new NorthwindContext();
    var req = ctx.Products.OrderByDescending(p => p.OrderDetails.Sum(o => o.Quantity));
    Console.WriteLine(req.ToQueryString());
    var p1 = req.First();
    Console.WriteLine(p1.ProductName);
    chrono.Stop();
    Console.WriteLine(chrono.ElapsedMilliseconds + "ms");


    chrono = Stopwatch.StartNew();
    ctx = new NorthwindContext();
    var req2 = ctx.OrderDetails
        .GroupBy(od => od.ProductId)
        .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(l => l.Quantity) })
        .OrderByDescending(r => r.TotalQuantity);
    var p3 = ctx.Products.Find(req2.First().ProductId);
    Console.WriteLine(req2.ToQueryString());
    Console.WriteLine(p3.ProductName);
    chrono.Stop();
    Console.WriteLine(chrono.ElapsedMilliseconds + "ms");
}
//AfficherStatistiques();


void grosClient()
{
    var chrono = Stopwatch.StartNew();
    NorthwindContext context = new NorthwindContext();
    var req = context.Invoices
        .GroupBy(o => o.CustomerName)
        .Select(c => new { CustomerName = c.Key, totalAmount = c.Sum(l => l.ExtendedPrice) })        
        .OrderByDescending(r => r.totalAmount);
    Console.WriteLine(req.ToQueryString());
    var c3 = req.First();
    Console.WriteLine(c3.CustomerName);
    chrono.Stop();
    Console.WriteLine(chrono.ElapsedMilliseconds + "ms");



    chrono = Stopwatch.StartNew();
    context = new NorthwindContext();
    var req2 = context.Orders
        .Join(
        context.OrderDetails, 
        o => o.OrderId, 
        od => od.OrderId,
        (o, od) => new
        {
            CustomerName = o.Customer.CompanyName,
            ExtendedPrice = od.UnitPrice * od.Quantity * ((1m - (decimal)od.Discount))
        })
        .GroupBy(o => o.CustomerName)
        .Select(g => new { CustomerName = g.Key, TotalAmount = g.Sum(od => od.ExtendedPrice) })
        .OrderByDescending(r => r.TotalAmount);
    
        
        
        Console.WriteLine(req.ToQueryString());
    var c4 = req.First();
    Console.WriteLine(c3.CustomerName);
    chrono.Stop();
    Console.WriteLine(chrono.ElapsedMilliseconds + "ms");
}

grosClient();
using HPlusSport.API.Models;

namespace HPlusSport.API.Services;

public class ProductService
{
    private readonly List<Product> _products = new();

    public IEnumerable<Product> GetAll() => _products;

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public void Add(Product product)
    {
        product.Id = _products.Count + 1;
        _products.Add(product);
    }

    public bool Update(int id, Product updatedProduct)
    {
        var product = GetById(id);
        if (product == null) return false;

        product.ProductName = updatedProduct.ProductName;
        product.Price = updatedProduct.Price;
        product.Qty = updatedProduct.Qty;

        return true;
    }

    public bool Delete(int id)
    {
        var product = GetById(id);
        if (product == null) return false;

        _products.Remove(product);
        return true;
    }
}
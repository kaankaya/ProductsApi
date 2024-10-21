using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsApi.DTO;
using ProductsApi.Models;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")] //localhost/api/products
    [ApiController]
    public class ProductsController : ControllerBase
    {
       private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet] //localhost/api/products => GET
        public async Task<IActionResult> GetProducts()
        {
           var products = await _context.Products.Where(x=>x.IsActive == true).Select(p=> new ProductDTO
           {
               ProductId = p.ProductId,
               ProductName = p.ProductName,
               Price = p.Price,
           }).ToListAsync();
            return Ok(products);
        }
        /*
         yukarıda öncelike products değişkenimizi tanımladık.daha sonra _context içerisinden(yani veritabanından) Where komutu ile aktif olanları döndük ve 
        select ile seçtik daha sonra seçtiğimiz ürünleri new ProductDTO diyerek dto nesnemize aktardık ve aktardığımız veriyi bir listeye çevirdik.
        böylelikle kullanıcıya sadece istediğimiz alanları gösterdik.
         */

        [HttpGet("{id}")]//localhost/api/products/1 => GET
        public async Task<IActionResult> GetProduct(int? id)
        {
            if( id == null)
            {
                return NotFound();
            }
            var p = await _context
                          .Products
                          .Select(p => ProductToDto(p))
                          .FirstOrDefaultAsync(x => x.ProductId == id);
            if(p is null)
            {
                return NotFound();
            }
            return Ok(p);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct),new { id = entity.ProductId}, entity);
        }

        [HttpPut("{id}")] //localhost/api/produtcs/5 => put
        public async Task<IActionResult> UpdateProduct(int id, Product entity) //güncelleme için hem id değeri aldık dışarıdan geleceği için ,hemde product nesnemizi aldık
        {
            if(id != entity.ProductId)
            {
                return BadRequest();
            }
            var product = await _context.Products.FirstOrDefaultAsync(x=>x.ProductId == id); 
            if(product is null) //veritabanında böyle bir id li değer yok ise notfound
            {
                return NotFound();
            }
            product.ProductName = entity.ProductName;
            product.Price = entity.Price;
            product.IsActive = entity.IsActive;

            //yukarıda verileri işaretledik şimdi veritabanı için işlemi yapacağız

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent(); //status 204

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x=>x.ProductId == id);
            if(product is null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return NoContent();
            }
            return NoContent();
        }

        //burada method tanımladık çünkü bunları kodun çoğu yerinde kullanacagımız için static bir method tanımladık private olarak işaretledik
        // ProducDTO içeren bir method paremetre olarak da bir product nesnesi alıyor
        private static ProductDTO ProductToDto(Product p)
        {
            return new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,

            };
        }
    }
}

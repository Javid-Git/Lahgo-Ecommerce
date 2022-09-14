using LAHGO.Core;
using LAHGO.Core.Repositories;
using LAHGO.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly ProductRepository _productRepository;
        private readonly ColorRepository _colorRepository;
        private readonly SizeRepository _sizeRepository;
        private readonly SettingRepository _settingRepository;
        private readonly ProductColorSizeRepository _productColorSizeRepository;
        private readonly PhotoRepository _photoRepository;
        private readonly BasketRepository _basketRepository;
        private readonly OrderRepository _orderRepository;
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public ICategoryRepository CategoryRepository => _categoryRepository != null ? _categoryRepository : new CategoryRepository(_context);

        public IProductRepository ProductRepository => _productRepository != null ? _productRepository : new ProductRepository(_context);
        public IProductColorSizeRepository ProductColorSizeRepository => _productColorSizeRepository != null ? _productColorSizeRepository : new ProductColorSizeRepository(_context);
        public ISizeRepository SizeRepository => _sizeRepository != null ? _sizeRepository : new SizeRepository(_context);
        public IColorRepository ColorRepository => _colorRepository != null ? _colorRepository : new ColorRepository(_context);
        public ISettingRepository SettingRepository => _settingRepository != null ? _settingRepository : new SettingRepository(_context);
        public IPhotoRepository PhotoRepository => _photoRepository != null ? _photoRepository : new PhotoRepository(_context);
        public IBasketRepository BasketRepository => _basketRepository != null ? _basketRepository : new BasketRepository(_context);
        public IOrderRepository OrderRepository => _orderRepository != null ? _orderRepository : new OrderRepository(_context);

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

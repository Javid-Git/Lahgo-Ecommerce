﻿using LAHGO.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LAHGO.Core
{
    public interface IUnitOfWork
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
         IProductColorSizeRepository ProductColorSizeRepository { get; }
        ISizeRepository SizeRepository { get; }
        IColorRepository ColorRepository { get; }
        ISettingRepository SettingRepository { get; }
        IPhotoRepository PhotoRepository { get; }
        IBasketRepository BasketRepository { get; }
        Task<int> CommitAsync();
        int Commit();
       

    }
}

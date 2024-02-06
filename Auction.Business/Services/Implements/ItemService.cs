﻿using Auction.Business.DTOs.ItemDTOs;
using Auction.Business.Exceptions.Common;
using Auction.Business.Repositories.Interfaces;
using Auction.Business.Services.Interfaces;
using Auction.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Services.Implements;

public class ItemService : IItemService
{
    readonly IItemRepository _repo;
    readonly ICategoryRepository _catRepo;
    readonly IMapper _mapper;
    readonly IHttpContextAccessor _contextAccessor;
    readonly string _userId;

    public ItemService(IItemRepository repo, IMapper mapper, IHttpContextAccessor contextAccessor, ICategoryRepository catRepo)
    {
        _repo = repo;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        if (_contextAccessor.HttpContext.User.Claims.Any())
        {
            _userId = _contextAccessor.HttpContext?.User?.Claims?.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException();
        }
        _catRepo = catRepo;
    }

    public async Task CreateAsync(ItemCreateDTO dto)
    {
        var data = _mapper.Map<Item>(dto);
        if (!await _catRepo.IsExistAsync(r => r.Id == dto.CategoryId))
            throw new NotFoundException<Category>();
        data.SellerId = _userId;
        data.CurrentPrice = data.StartingPrice;
        await _repo.CreateAsync(data);
        await _repo.SaveAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var data = await _checkId(id);
        _repo.Delete(data);
        await _repo.SaveAsync();
    }

    public IEnumerable<ItemListDTO> GetAll()
        =>_mapper.Map<IEnumerable<ItemListDTO>>(_repo.GetAll(includes: new[] { "Seller", "Category" }));

    public async Task<ItemDetailsDTO> GetByIdAsync(int id)
    {
        var data = await _checkId(id, true);
        return _mapper.Map<ItemDetailsDTO>(data);
    }

    public async Task UpdateAsync(int id, ItemUpdateDTO dto)
    {
        var data = await _checkId(id);
        data = _mapper.Map(dto, data);
        await _repo.SaveAsync();
    }

    public async Task SoftDelete(int id)
    {
        var data = await _checkId(id);
        data.IsDeleted = true;
        await _repo.SaveAsync();
    }

    public async Task ReverseSoftDelete(int id)
    {
        var data = await _checkId(id);
        data.IsDeleted = false;
        await _repo.SaveAsync();
    }

    async Task<Item> _checkId(int id, bool isTrack = false)
    {
        if (id <= 0) throw new ArgumentException();
        var data = await _repo.GetByIdAsync(id, isTrack, "Seller", "Category");
        if (data == null) throw new NotFoundException<Item>();
        return data;
    }
}

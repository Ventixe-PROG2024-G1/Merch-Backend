

using Application.Data.Repository;
using Application.Domain;
using Application.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Services;

public interface IMerchService
{
    Task<MerchViewModel?> CreateAsync(CreateMerch merch);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<MerchViewModel>> GetAllAsync();
    Task<MerchViewModel?> GetByIdAsync(Guid id);
    Task<IEnumerable<MerchViewModel>> SetCache();
    Task<bool> UpdateAsync(Guid id, CreateMerch merch);
}

public class MerchService(IMerchRepository repository, IMemoryCache cache) : IMerchService
{
    private readonly IMerchRepository _merchRepository = repository;
    private readonly IMemoryCache _cache = cache;
    private const string _cacheKey_All = "Ticket_All";

    public async Task<IEnumerable<MerchViewModel>> GetAllAsync()
    {
        if (_cache.TryGetValue(_cacheKey_All, out IEnumerable<MerchViewModel>? cachedMerch))
            return cachedMerch!;

        return await SetCache();

    }

    public async Task<MerchViewModel?> GetByIdAsync(Guid id)
    {
        var merch = await _merchRepository.GetByIdAsync(id);
        return merch is null ? null : MapToViewModel(merch);
    }

    public async Task<MerchViewModel?> CreateAsync(CreateMerch merch)
    {
        var entity = new MerchEntity
        {
            Id = Guid.NewGuid(),
            Price = merch.Price,
            Name = merch.Name,
            ImageId = merch.ImageId,
            EventId = merch.EventId,
        };

        var result = await _merchRepository.AddAsync(entity);
        if (result)
        {
            await SetCache();
            return MapToViewModel(entity);
        }
        return null;

    }



    public async Task<bool> DeleteAsync(Guid id)
    {
        var result = await _merchRepository.DeleteAsync(x => x.Id == id);
        if (result)
            await SetCache();
        return result;
    }



    public async Task<bool> UpdateAsync(Guid id, CreateMerch merch)
    {
        var existingMerch = await _merchRepository.GetByIdAsync(id);
        if (existingMerch == null)
            return false;

        existingMerch.Price = merch.Price;
        existingMerch.Name = merch.Name;
        existingMerch.ImageId = merch.ImageId;
        existingMerch.EventId = merch.EventId;

        var result = await _merchRepository.UpdateAsync(existingMerch);
        if (result)
            await SetCache();
        return result;
    }



    private static MerchViewModel MapToViewModel(MerchEntity entity)
    {
        return new MerchViewModel
        {
            Id = entity.Id,
            Price = entity.Price,
            Name = entity.Name,
            ImageId = entity.ImageId,
            EventId = entity.EventId
        };
    }


    public async Task<IEnumerable<MerchViewModel>> SetCache()
    {
        _cache.Remove(_cacheKey_All);
        var entites = await _merchRepository.GetAllAsync(sortBy: x => x.Id);
        var merch = entites.Select(entity => new MerchViewModel
        {
            Id = entity.Id,
            Price = entity.Price,
            Name = entity.Name,
            ImageId = entity.ImageId,
            EventId = entity.EventId
        });

        _cache.Set(_cacheKey_All, merch, TimeSpan.FromMinutes(30));
        return merch;
    }
}

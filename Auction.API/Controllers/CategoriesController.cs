﻿using Auction.Business.DTOs.CategoryDTOs;
using Auction.Business.Exceptions.Category;
using Auction.Business.Services.Implements;
using Auction.Business.Services.Interfaces;
using Auction.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    readonly ICategoryService _service;
    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }
    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAll());
    }
    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            return Ok(await _service.GetByIdAsync(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost]
    [Route("Create")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> Create(CategoryCreateDTO topic)
    {
        try
        {
            await _service.CreateAsync(topic);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (CategoryExistException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("Delete/{id}")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPatch("SoftDelete/{id}")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> SoftDelete(int id)
    {
        try
        {
            await _service.SoftDelete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPatch("ReverseSoftDelete/{id}")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> ReverseSoftDelete(int id)
    {
        try
        {
            await _service.ReverseSoftDelete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut("Update/{id}")]
    [Authorize(Roles = nameof(Roles.Admin))]
    public async Task<IActionResult> Update(int id, CategoryUpdateDTO dto)
    {
        try
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }
        catch (CategoryExistException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

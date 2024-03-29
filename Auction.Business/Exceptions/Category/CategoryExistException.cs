﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Business.Exceptions.Category;

public class CategoryExistException : Exception, IBaseException
{
    public string ErrorMessage { get; set; }
    public int StatusCode => StatusCodes.Status409Conflict;
    public CategoryExistException()
    {
        ErrorMessage = "Category already exists";
    }
    public CategoryExistException(string? message)
    {
        ErrorMessage = message;
    }
}

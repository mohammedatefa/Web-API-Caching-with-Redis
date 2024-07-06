using AutoMapper;
using Caching_with_Redis.Controllers.Products.Request;
using Caching_with_Redis.Models;
using Caching_with_Redis.Repositories;
using Caching_with_Redis.Utalitis.UOW;
using Caching_with_Redis.Utalitis.Validation;
using Microsoft.AspNetCore.Mvc;

namespace Caching_with_Redis.Controllers.Products
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IGenericRepository<Product> repository;

        public ProductController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            repository = unitOfWork.Repository<Product>();
        }

        [HttpGet("Product")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await repository.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("AllProduct")]
        public async Task<IActionResult> GetAll()
        {
            var product = await repository.GetAllAsync();
            return Ok(product);
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> Add(ProuductRequest request)
        {
            var validation = new ProductValidations();
            var validationResult = validation.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage);
                return BadRequest(errors);
            }
            var value = mapper.Map<Product>(request);
            await repository.AddAsync(value);
            await unitOfWork.SaveChanges();

            return Ok(new
            {
                Message = $"Product {request.Title} added Succeded "
            });
        }

        [HttpPost("UpdateProduct")]
        public async Task<IActionResult> Update(int Id,ProuductRequest request)
        {
            var validation = new ProductValidations();
            var validationResult = validation.Validate(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.ToDictionary(error => error.PropertyName, error => error.ErrorMessage);
                return BadRequest(errors);
            }
            var value = mapper.Map<Product>(request);
            await repository.UpdateAsync(Id,value);
            await unitOfWork.SaveChanges();

            return Ok(new
            {
                Message = $"Product {request.Title} Updated Succeded "
            });
        }


        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> Delete(int id)
        {
            await repository.DeleteAsync(id);
            await unitOfWork.SaveChanges();
            return Ok( new
            {
                Message = $"Product With Id {id} Deleted Succeded "
            });
        }


    }
}

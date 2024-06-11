using AutoMapper;
using E_Commerce.Apis.DTOs;
using E_Commerce.Apis.Errors;
using E_Commerce.Apis.Helpers;
using E_Commerce.Core.Entities.Product_Module;
using E_Commerce.Core.Interfaces;
using E_Commerce.Core.Interfaces.Repositories;
using E_Commerce.Core.Specifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Apis.Controllers
{
   
    public class ProductsController : ApiBaseController
    {
        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper,IUnitOfWork unitOfWork)
        {
           
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
            
        }




        //[Authorize]
       // [CachedAttribute(600)]
        [HttpGet]
        //Improving Swagger Documentation

        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiExceptionResponse), 404)]
        
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams @params) 
        { 
            var spec=new ProductWithBrandAndTypeSpec(@params);
        var Products= await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var MappedProducts = _mapper.Map<IReadOnlyList<Product>,IReadOnlyList< ProductToReturnDto>>(Products);
            var CountSpec = new ProductWithFiltrationForCountAsync(@params);
            var count= await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);

            var ReturnedObject = new Pagination<ProductToReturnDto>()
            {
                 PageSize=@params.PageSize,
                  PageIndex=@params.PageIndex,
                   Data = MappedProducts,
                   Count= count
            };
            return Ok(ReturnedObject);
        
        
        }

      //  [CachedAttribute(600)]
        [HttpGet("{id}")]
        //Improving Swagger Documentation

        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(ApiExceptionResponse),404)]

        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {

            var spec = new ProductWithBrandAndTypeSpec(id);
            var Product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (Product == null) return NotFound(new ApiExceptionResponse(404));
            var MappedProduct = _mapper.Map<Product, ProductToReturnDto>(Product);

            return Ok(MappedProduct);


        }



        //[CachedAttribute(600)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands= await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return Ok(Brands);
        }


       // [CachedAttribute(600)]
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();

            return Ok(Types);
        }

    }
}

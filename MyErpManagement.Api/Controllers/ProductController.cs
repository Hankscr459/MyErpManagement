using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Examples.Shared;
using MyErpManagement.Api.Helpers;
using MyErpManagement.Core.Dtos.Products.Request;
using MyErpManagement.Core.Dtos.Products.Response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.ProductsModule.Entities;
using MyErpManagement.Core.Modules.ProductsModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using MyErpManagement.DataBase.Helpers;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Controllers
{
    public class ProductController(
        IUnitOfWork unitOfWork,
        IMapper mapper
    ) : BaseApiController
    {
        [HttpPost]
        [HasPermission(PermissionKeysConstant.Product.CreateProduct.Key)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status409Conflict)]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConclictResponseExample))]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            await unitOfWork.ProductRepository.AddAsync(new Product
            {
                Name = createProductRequestDto.Name,
                ProductCategoryId = createProductRequestDto.ProductCategoryId,
                Code = createProductRequestDto.Code,
                Specification = createProductRequestDto.Specification,
                PurchasePrice = createProductRequestDto.PurchasePrice,
                SalesPrice = createProductRequestDto.SalesPrice,
                CreatedBy = User.GetUserId()
            });
            if (!await unitOfWork.Complete())
            {
                return BadRequest("新增商品失敗");
            }
            return NoContent();
        }

        [HttpGet("{productId}")]
        [HasPermission(PermissionKeysConstant.Product.ReadProductList.Key)]
        public async Task<ActionResult<ProductListItemDto>> ReadProduct(Guid productId)
        {
            var product = await unitOfWork.ProductRepository.GetFirstOrDefaultAsync(
                    p => p.Id == productId,
                    "ProductCategory"
                );
            if (product is null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.NotFound, "查無此商品"));
            }
            return Ok(mapper.Map<ProductListItemDto>(product));
        }

        [HttpGet]
        [HasPermission(PermissionKeysConstant.Product.ReadProductList.Key)]
        public async Task<ActionResult<ProductListResponseDto>> ProductList([FromQuery] ProductListQueryRequestDto productListQueryRequestDto)
        {
            var productListQuery = mapper.Map<ProductListQueryModel>(productListQueryRequestDto);
            var ProductListQueryable = await unitOfWork.ProductRepository.FindProductsByQuery(productListQuery, p =>
                mapper.Map<ProductListItemDto>(p));
            var list = await ProductListQueryable.ToPagedListAsync(productListQuery.Page, productListQuery.Limit);
            return mapper.Map<ProductListResponseDto>(list);
        }

        [HttpPost("categories")]
        [HasPermission(PermissionKeysConstant.Product.CreateProductCategory.Key)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status409Conflict)]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConclictResponseExample))]
        public async Task<ActionResult> CreateCategory([FromBody] CreateProductCategoryRequestDto createProductCategoryRequestDto)
        {
            await unitOfWork.ProductCategoryRepository.AddAsync(new ProductCategory
            {
                Name = createProductCategoryRequestDto.Name,
                ParentId = createProductCategoryRequestDto.ParentId,
                CreatedBy = User.GetUserId()
            });
            if (!await unitOfWork.Complete())
            {
                return BadRequest("新增商品分類失敗");
            }
            return NoContent();
        }
    }
}

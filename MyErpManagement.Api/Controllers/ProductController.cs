using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Examples.Product;
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

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <param name="createProductRequestDto"></param>
        /// <returns></returns>
        [HttpPost]
        [HasPermission(PermissionKeysConstant.Product.CreateProduct.Key)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConclictResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateProductFailResponseExample))]
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductRequestDto createProductRequestDto)
        {
            var product = mapper.Map<Product>(createProductRequestDto);
            product.CreatedBy = User.GetUserId();
            await unitOfWork.ProductRepository.AddAsync(product);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateProduct));
            }
            return NoContent();
        }

        /// <summary>
        /// 查看商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet("{productId}")]
        [HasPermission(PermissionKeysConstant.Product.ReadProduct.Key)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProductNotFoundResponseExample))]
        public async Task<ActionResult<ProductListItemDto>> ReadProduct(Guid productId)
        {
            var product = await unitOfWork.ProductRepository.GetFirstOrDefaultAsync(
                    p => p.Id == productId,
                    "ProductCategory"
                );
            if (product is null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Product));
            }
            return Ok(mapper.Map<ProductListItemDto>(product));
        }

        /// <summary>
        /// 查看商品清單頁面
        /// </summary>
        /// <param name="productListQueryRequestDto"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="updateProductRequestDto"></param>
        /// <returns></returns>
        [HttpPut("{productId}")]
        [HasPermission(PermissionKeysConstant.Product.UpdateProduct.Key)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProductNotFoundResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateProductFailResponseExample))]
        public async Task<ActionResult> UpdateProduct(Guid productId, UpdateProductRequestDto updateProductRequestDto)
        {
            var product = await unitOfWork.ProductRepository.GetFirstOrDefaultAsync(p => p.Id == productId);
            if (product is null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.Product));
            }
            Product productEntity = updateProductRequestDto.Adapt(product);
            unitOfWork.ProductRepository.Update(updateProductRequestDto.Adapt(product));
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.BadRequest.FailToUpdateProduct));
            }
            return NoContent();
        }

        /// <summary>
        /// 新增商品分類
        /// </summary>
        /// <param name="createProductCategoryRequestDto"></param>
        /// <returns></returns>
        [HttpPost("category")]
        [HasPermission(PermissionKeysConstant.Product.CreateProductCategory.Key)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status409Conflict)]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConclictResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateProductCategoryFailResponseExample))]
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
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateProductCategory));
            }
            return NoContent();
        }

        /// <summary>
        /// 查看單筆商品分類
        /// </summary>
        /// <param name="productCategoryId"></param>
        /// <returns></returns>
        [HttpGet("category/{productCategoryId}")]
        [HasPermission(PermissionKeysConstant.ProductCategory.ReadProductCategory.Key)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProductCategoryNotFoundResponseExample))]
        public async Task<ActionResult<ProductCategoryResponseDto>> ReadProductCategory(Guid productCategoryId)
        {
            var product = await unitOfWork.ProductCategoryRepository.ProjectTo(
                    p => p.Id == productCategoryId,
                    p => mapper.Map<ProductCategoryResponseDto>(p)
                ).FirstOrDefaultAsync();
            if (product is null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.ProductCategory));
            }
            return Ok(product);
        }

        /// <summary>
        /// 商品分類清單
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        [HasPermission(PermissionKeysConstant.ProductCategory.ReadProductCategories.Key)]
        [ProducesResponseType(typeof(ProductCategoryTreeResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductCategoryListItemResponseDto>>> ReadProductCategories()
        {
            return Ok(await unitOfWork.ProductCategoryRepository.ProjectAsync(
                p => true,
                p => mapper.Map<ProductCategoryListItemResponseDto>(p)
            ));
        }

        /// <summary>
        /// 商品分類樹狀結構
        /// </summary>
        /// <returns></returns>
        [HttpGet("category/tree")]
        [HasPermission(PermissionKeysConstant.ProductCategory.ReadProductCategoryTree.Key)]
        [ProducesResponseType(typeof(ProductCategoryTreeResponseDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductCategoryTreeResponseDto>> ReadProductCategoryTree()
        {
            return Ok(await unitOfWork.ProductCategoryRepository.GetCategoryTreeAsync());
        }

        /// <summary>
        /// 修改商品分類
        /// </summary>
        /// <param name="productCategoryId"></param>
        /// <param name="updateProductCategoryRequestDto"></param>
        /// <returns></returns>
        [HttpPut("category/{productCategoryId}")]
        [HasPermission(PermissionKeysConstant.ProductCategory.UpdateProductCategory.Key)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(ProductCategoryNotFoundResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(UpdateProductCategoryFailResponseExample))]
        public async Task<ActionResult> UpdateProductCategory(Guid productCategoryId, UpdateProductCategoryRequestDto updateProductCategoryRequestDto)
        {
            var productCategory = await unitOfWork.ProductCategoryRepository.GetFirstOrDefaultAsync(p => p.Id == productCategoryId);
            if (productCategory is null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.NotFound, ResponseTextConstant.NotFound.ProductCategory));
            }
            unitOfWork.ProductCategoryRepository.Update(updateProductCategoryRequestDto.Adapt(productCategory));
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToUpdateProductCategory));
            }
            return NoContent();
        }
    }
}

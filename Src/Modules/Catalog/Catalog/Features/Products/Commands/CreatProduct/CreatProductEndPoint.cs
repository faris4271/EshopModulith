using Carter;
using CatalogContract.Dtos;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static Catalog.Features.Products.Commands.CreatProduct.CreatProductCommandHandler;

namespace Catalog.Features.Products.Commands.CreatProduct
{
    public class CreatProductEndPoint : ICarterModule
    {

        public void AddRoutes(IEndpointRouteBuilder app)
        {
            // Manually read the multipart form to avoid the default form-to-model mapper limits
            app.MapPost("api/products", async (HttpRequest request, [FromServices] ISender send) =>
            {
                var form = await request.ReadFormAsync();



                // Expecting a JSON payload for the product in the "Product" field
                var productJson = form["Product"].FirstOrDefault();
                var productDto = productJson is null
                    ? new CreatProductDto()
                    : Newtonsoft.Json.JsonConvert.DeserializeObject<CreatProductDto>(productJson)!;

                for (int i = 0; i < productDto.Variations.Count; i++)
                {

                    var variantFiles = form.Files.GetFiles($"Variations[{i}].NewImages");
                    var varinatnThumbnail = form.Files.GetFile($"Variations[{i}].ThumbnailImage");
                    var formFiles = new FormFileCollection();
                    var fileCollection = new FormFileCollection();
                    fileCollection.AddRange(variantFiles);

                    formFiles.Add(varinatnThumbnail);


                    productDto.Variations[i].NewImages = fileCollection;
                    productDto.Variations[i].ThumbnailImage = formFiles;
                }

                var productForm = new ProductForm
                {
                    Product = productDto,
                    ThumbnailImage = form.Files.GetFile("ThumbnailImage"),
                    ProductImages = form.Files.Where(f => f.Name == "ProductImages").ToList(),
                    ProductDocuments = form.Files.Where(f => f.Name == "ProductDocuments").ToList(),
                };

                var command = new CreatProductCommand(productForm);
                var respons = await send.Send(command);

                return respons.Match(Results.Created, Results.BadRequest);
            }).WithTags("product").DisableAntiforgery();

        }
    }
}

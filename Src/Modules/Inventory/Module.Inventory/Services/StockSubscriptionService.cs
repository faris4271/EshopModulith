//using Microsoft.EntityFrameworkCore;
//using Module.Inventory.Data;
//using Module.Inventory.Models;
//using Shared.Abstraction;

//namespace Module.Inventory.Services
//{
//    public class StockSubscriptionService : IStockSubscriptionService
//    {
//        private readonly IGenericeRepository<ProductBackInStockSubscription, InventoryDbContext> _productBackInStockSubscriptionRepository;
//        private readonly IRepository<Product> _productRepository;
//        private readonly IEmailSender _emailSender;


//        public StockSubscriptionService(IRepository<ProductBackInStockSubscription> backInStockSubscriptionRepository, IEmailSender emailSender, IRepository<Product> productRepository)
//        {
//            _productBackInStockSubscriptionRepository = backInStockSubscriptionRepository;
//            _emailSender = emailSender;
//            _productRepository = productRepository;
//        }

//        public async Task ProductBackInStockSendNotificationsAsync(long productId)
//        {
//            var subscriptions = await _productBackInStockSubscriptionRepository
//                .Query()
//                .Where(o => o.ProductId == productId)
//                .ToListAsync();

//            var product = await _productRepository
//                .Query()
//                .Where(o => o.Id == productId)
//                .Include(o => o.ThumbnailImage)
//                .FirstOrDefaultAsync();

//            var emailBody = await _viewRender.RenderViewToStringAsync("/Areas/Inventory/Views/EmailTemplates/ProductBackInStockEmail.cshtml", product);
//            var emailSubject = $"Back in stock";

//            foreach (var subscription in subscriptions)
//            {
//                await _emailSender.SendEmailAsync(subscription.CustomerEmail, emailSubject, emailBody, true);

//                _productBackInStockSubscriptionRepository.Remove(subscription);
//            }

//            await _productBackInStockSubscriptionRepository.SaveChangesAsync();
//        }

//        public async Task ProductBackInStockSubscribeAsync(long productId, string customerEmail)
//        {
//            var subscription = new ProductBackInStockSubscription
//            {
//                ProductId = productId,
//                CustomerEmail = customerEmail
//            };

//            _productBackInStockSubscriptionRepository.Add(subscription);

//            await _productBackInStockSubscriptionRepository.SaveChangesAsync();
//        }
//    }
//}

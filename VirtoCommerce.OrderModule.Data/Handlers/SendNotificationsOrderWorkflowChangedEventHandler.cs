using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common.Events;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.OrderModule.Data.Notifications;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Notifications;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;

namespace VirtoCommerce.OrderModule.Data.Handlers
{
    public class SendNotificationsOrderWorkflowChangedEventHandler : IEventHandler<OrderChangedEvent>
    {
        private readonly INotificationManager _notificationManager;
        private readonly IStoreService _storeService;
        private readonly IMemberService _memberService;
        private readonly ISettingsManager _settingsManager;
        private readonly ISecurityService _securityService;        

        public SendNotificationsOrderWorkflowChangedEventHandler(INotificationManager notificationManager, IStoreService storeService, IMemberService memberService, ISettingsManager settingsManager, ISecurityService securityService, ICustomerOrderService customerOrderService)
        {
            _notificationManager = notificationManager;
            _storeService = storeService;
            _memberService = memberService;
            _settingsManager = settingsManager;
            _securityService = securityService;
        }

        public virtual async Task Handle(OrderChangedEvent message)
        {
            if (_settingsManager.GetValue("Order.SendOrderNotifications", true))
            {
                foreach (var changedEntry in message.ChangedEntries)
                {
                    await TryToSendOrderNotificationsAsync(changedEntry);
                }
            }
        }

        protected virtual async Task TryToSendOrderNotificationsAsync(GenericChangedEntry<CustomerOrder> changedEntry)
        {
            // Collection of order notifications
            var notifications = new List<OrderEmailNotificationBase>();

            if (changedEntry.EntryState == EntryState.Added && !changedEntry.NewEntry.IsPrototype)
            {
                var notificationWorkflow = _notificationManager.GetNewNotification<OrderWorkflowNotification>(changedEntry.NewEntry.StoreId, "Store", changedEntry.NewEntry.LanguageCode);
                notifications.Add(notificationWorkflow);
            }

            foreach (var notification in notifications)
            {
                notification.CustomerOrder = changedEntry.NewEntry;
                await SetNotificationParametersAsync(notification, changedEntry);
                _notificationManager.ScheduleSendNotification(notification);
            }
        }

        /// <summary>
        /// Set base notification parameters (sender, recipient, isActive)
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="changedEntry"></param>
        protected virtual async Task SetNotificationParametersAsync(Notification notification, GenericChangedEntry<CustomerOrder> changedEntry)
        {
            var order = changedEntry.NewEntry;
            var store = _storeService.GetById(order.StoreId);

            notification.IsActive = true;

            notification.Sender = store.Email;
            notification.Recipient = await GetOrderRecipientEmailAsync(order);

            // Allow to filter notification log either by customer order or by subscription
            if (string.IsNullOrEmpty(order.SubscriptionId))
            {
                notification.ObjectTypeId = "CustomerOrder";
                notification.ObjectId = order.Id;
            }
            else
            {
                notification.ObjectTypeId = "Subscription";
                notification.ObjectId = order.SubscriptionId;
            }
        }

        protected virtual async Task<string> GetOrderRecipientEmailAsync(CustomerOrder order)
        {
            var email = GetOrderAddressEmail(order) ?? await GetCustomerEmailAsync(order.CustomerId);
            return email;
        }

        protected virtual string GetOrderAddressEmail(CustomerOrder order)
        {
            var email = order.Addresses?.Select(x => x.Email).FirstOrDefault(x => !string.IsNullOrEmpty(x));
            return email;
        }

        protected virtual async Task<string> GetCustomerEmailAsync(string customerId)
        {
            var user = await _securityService.FindByIdAsync(customerId, UserDetails.Reduced);

            var contact = user != null
                ? _memberService.GetByIds(new[] { user.MemberId }).OfType<Contact>().FirstOrDefault()
                : _memberService.GetByIds(new[] { customerId }).OfType<Contact>().FirstOrDefault();

            var email = contact?.Emails?.FirstOrDefault(x => !string.IsNullOrEmpty(x)) ?? user?.Email;
            return email;
        }
    }
}

using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Cats_Cafe_Accounting_System.Models
{
    public class NavigationChangedRequestMessage : ValueChangedMessage<NavigationModel>
    {
        public NavigationChangedRequestMessage(NavigationModel model) : base(model) { }
    }
}
